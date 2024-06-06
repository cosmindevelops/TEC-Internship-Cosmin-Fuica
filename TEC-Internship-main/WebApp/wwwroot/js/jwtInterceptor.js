document.addEventListener('DOMContentLoaded', function () {
    const originalFetch = fetch;
    window.fetch = async function (url, options = {}) {
        const token = AuthService.getToken();
        if (token) {
            options.headers = {
                ...options.headers,
                'Authorization': `Bearer ${token}`
            };
        }
        return originalFetch(url, options);
    };
});