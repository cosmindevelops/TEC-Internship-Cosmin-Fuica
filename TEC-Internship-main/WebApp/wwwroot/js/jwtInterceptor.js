//document.addEventListener('DOMContentLoaded', function () {
//    const originalFetch = fetch;
//    window.fetch = async function (url, options = {}) {
//        options = options || {};
//        options.headers = options.headers || {};

//        const token = AuthService.getToken();
//        console.log('Token:', token);  // Debug log
//        if (token) {
//            options.headers['Authorization'] = `Bearer ${token}`;
//            console.log('Authorization header set');  // Debug log
//        } else {
//            console.log('No token found');
//        }

//        const response = await originalFetch(url, options);
//        console.log('Response status:', response.status);
//        if (!response.ok) {
//            const errorText = await response.text();
//            console.log('Response error text:', errorText);
//        }

//        return response;
//    };
//});