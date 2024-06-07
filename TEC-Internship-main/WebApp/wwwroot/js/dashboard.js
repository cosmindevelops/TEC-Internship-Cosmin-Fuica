document.addEventListener('DOMContentLoaded', function () {
    const username = AuthService.getUsername();
    document.getElementById('username-placeholder').textContent = username || 'User';
});