class AuthService {
    static login(model) {
        return fetch('/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(model)
        }).then(response => response.json())
            .then(data => {
                if (data.token && data.username) {
                    localStorage.setItem('token', data.token);
                    localStorage.setItem('username', data.username);
                }
                return data;
            });
    }

    static register(model) {
        return fetch('/auth/register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(model)
        }).then(response => response.json());
    }

    static logout() {
        localStorage.removeItem('token');
        localStorage.removeItem('username');
    }

    static getToken() {
        return localStorage.getItem('token');
    }

    static getUsername() {
        return localStorage.getItem('username');
    }
}

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

function login() {
    const model = {
        email: document.getElementById('login-email').value,
        password: document.getElementById('login-password').value
    };
    AuthService.login(model)
        .then(data => {
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

function register() {
    const model = {
        username: document.getElementById('register-username').value,
        email: document.getElementById('register-email').value,
        password: document.getElementById('register-password').value
    };
    AuthService.register(model)
        .then(data => {
            alert('Registration successful');
        })
        .catch(error => {
            alert('Registration failed');
        });
}

function logout() {
    AuthService.logout();
    window.location.href = '/auth';
}