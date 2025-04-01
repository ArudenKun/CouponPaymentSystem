// $(function () {
//     const sideBarNavLink = $('.sidebar .nav-link');
//    
//     function setActiveLink($link) {
//         // Remove active state from all links
//         sideBarNavLink.removeClass('active').removeAttr('aria-current');
//         $('.sidebar .regular-icon').removeClass('d-none');
//         $('.sidebar .filled-icon').addClass('d-none');
//
//         // If a link was provided, set it as active
//         if ($link && $link.length) {
//             $link.addClass('active').attr('aria-current', 'page');
//             $link.find('.regular-icon').addClass('d-none');
//             $link.find('.filled-icon').removeClass('d-none');
//         }
//     }
//
//     // Click event for nav links
//     sideBarNavLink.on('click', function (e) {
//         e.preventDefault();
//         window.location.href = $(this).attr('href');
//     });
//
//     // Check initial active state
//     function checkInitialActiveState() {
//         const currentPath = window.location.pathname.toLowerCase();
//         let $activeLink = null;
//
//         // Find matching link by URL path
//         sideBarNavLink.each(function () {
//             try {
//                 const linkUrl = new URL($(this).attr('href'), window.location.origin);
//                 if (currentPath === linkUrl.pathname.toLowerCase()) {
//                     $activeLink = $(this);
//                     return false; // break the loop
//                 }
//             } catch (e) {
//                 console.warn('Invalid URL in nav link:', $(this).attr('href'));
//             }
//         });
//
//         setActiveLink($activeLink);
//     }
//
//     // Run initial check
//     checkInitialActiveState();
//
//     // Handle back/forward navigation
//     $(window).on('popstate', checkInitialActiveState);
// });
//
// function logout() {
//     console.log("logout");
// }