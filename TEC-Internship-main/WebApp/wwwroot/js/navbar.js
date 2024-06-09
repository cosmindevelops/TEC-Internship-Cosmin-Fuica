/**
 * Highlights the active navigation link based on the current URL path.
 */
document.addEventListener('DOMContentLoaded', function () {
    // Get the current URL path
    var path = window.location.pathname;
    // Select all navigation links in the custom navbar
    var navLinks = document.querySelectorAll('.navbar-custom .nav-link');

    /**
     * Iterate through each navigation link
     * Add 'active' class to the link that matches the current URL path
     */
    navLinks.forEach(function (link) {
        if (link.getAttribute('href') === path) {
            link.classList.add('active');
        }
    });
});