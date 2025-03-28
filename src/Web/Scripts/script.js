$(function () {
    // Function to update active state
    function setActiveLink($link) {
        // Remove active state from all links
        $('.sidebar .nav-link').removeClass('active').removeAttr('aria-current');
        $('.sidebar .regular-icon').removeClass('d-none');
        $('.sidebar .filled-icon').addClass('d-none');

        // Set active state on clicked link
        $link.addClass('active').attr('aria-current', 'page');
        $link.find('.regular-icon').addClass('d-none');
        $link.find('.filled-icon').removeClass('d-none');

        // Update URL with the active link's data-nav-id
        const navId = $link.data('nav-id');
        const url = new URL(window.location);
        url.searchParams.set('active', navId);
        window.history.pushState({}, '', url);
    }

    // Click event for nav links
    $('.sidebar .nav-link').on('click', function (e) {
        e.preventDefault();
        setActiveLink($(this));
        window.location.href = $(this).attr('href');
    });

    // Check initial active state
    function checkInitialActiveState() {
        const currentPath = window.location.pathname.toLowerCase();
        let $activeLink = null;

        // Try to find matching link by URL path
        $('.sidebar .nav-link').each(function () {
            const linkPath = $(this).attr('href').toLowerCase();
            if (currentPath === linkPath) {
                $activeLink = $(this);
                return false;
            }
        });

        // If no match found, try by active param
        if (!$activeLink) {
            const urlParams = new URLSearchParams(window.location.search);
            const activeParam = urlParams.get('active');
            if (activeParam) {
                $activeLink = $('.sidebar .nav-link').filter(`[data-nav-id="${activeParam}"]`);
            }
        }

        // If still no match, use first item
        if (!$activeLink || !$activeLink.length) {
            $activeLink = $('.sidebar .nav-link').first();
        }

        setActiveLink($activeLink);
    }

    // Run initial check
    checkInitialActiveState();

    // Handle back/forward navigation
    $(window).on('popstate', checkInitialActiveState);
});

function logout() {
    console.log("logout");
}