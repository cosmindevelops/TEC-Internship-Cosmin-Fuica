/**
 * Displays the username in the designated placeholder.
 * If the username is not found, it displays 'User'.
 */
document.addEventListener('DOMContentLoaded', function () {
    // Get the username from the AuthService
    const username = AuthService.getUsername();
    // Set the text content of the username placeholder to the username or 'User' if not found
    document.getElementById('username-placeholder').textContent = username || 'User';
});