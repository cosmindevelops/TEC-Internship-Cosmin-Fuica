/**
 * AuthService class to handle authentication operations.
 */
class AuthService {

    /**
     * Logs in a user.
     * @param {Object} model - The login model containing email and password.
     * @returns {Promise<Object>} The response data from the login request.
     */
    static login(model) {
        return fetch('/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(model)
        }).then(response => response.json())
            // Store token and username in local storage if login is successful
            .then(data => {
                if (data.token && data.username) {
                    localStorage.setItem('token', data.token);
                    localStorage.setItem('username', data.username);
                }
                return data;
            });
    }

    /**
     * Registers a new user.
     * @param {Object} model - The registration model containing username, email, and password.
     * @returns {Promise<Object>} The response data from the registration request.
     */
    static register(model) {
        return fetch('/auth/register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(model)
        }).then(response => response.json());
    }

    /**
     * Logs out the user by removing the token and username from local storage.
     */
    static logout() {
        localStorage.removeItem('token');
        localStorage.removeItem('username');
    }

    /**
     * Gets the current authentication token.
     * @returns {string|null} The current authentication token.
     */
    static getToken() {
        return localStorage.getItem('token');
    }

    /**
     * Gets the current username.
     * @returns {string|null} The current username.
     */
    static getUsername() {
        return localStorage.getItem('username');
    }
}

/**
 * Toggles the display between the login and registration forms.
 */
function toggleForms() {
    var loginForm = document.getElementById('login-form');
    var registerForm = document.getElementById('register-form');
    if (loginForm.style.display === 'none') {
        loginForm.style.display = 'block';
        registerForm.style.display = 'none';
    } else {
        loginForm.style.display = 'none';
        registerForm.style.display = 'block';
    }
}

/**
 * Handles the login process.
 */
function login() {
    const model = {
        email: document.getElementById('login-email').value,
        password: document.getElementById('login-password').value
    };
    AuthService.login(model)
        .then(data => {
            // Redirect to the provided URL if login is successful
            if (data.redirectUrl) {
                window.location.href = data.redirectUrl;
            } else {
                alert('Login failed');
            }
        })
        .catch(error => {
            alert('Login failed');
        });
}

/**
 * Handles the registration process.
 */
function register() {
    const model = {
        username: document.getElementById('register-username').value,
        email: document.getElementById('register-email').value,
        password: document.getElementById('register-password').value
    };
    AuthService.register(model)
        .then(data => {
            // Log in the user automatically after registration
            return AuthService.login(model);
        })
        .then(loginData => {
            // Redirect to the provided URL if login after registration is successful
            if (loginData.redirectUrl) {
                window.location.href = loginData.redirectUrl;
            } else {
                alert('Login after registration failed');
            }
        })
        .catch(error => {
            alert('Registration failed');
        });
}

/**
 * Logs out the user and redirects to the authentication page.
 */
function logout() {
    AuthService.logout();
    window.location.href = '/auth';
}