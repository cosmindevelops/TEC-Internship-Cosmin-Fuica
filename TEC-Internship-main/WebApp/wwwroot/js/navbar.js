document.addEventListener('DOMContentLoaded', function () {
    var path = window.location.pathname;
    var navLinks = document.querySelectorAll('.navbar-custom .nav-link');

    navLinks.forEach(function (link) {
        if (link.getAttribute('href') === path) {
            link.classList.add('active');
        }
    });
});