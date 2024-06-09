document.addEventListener('DOMContentLoaded', function () {
    /**
     * Handle click on the create person button
     * Validates input fields and sends a POST request to create a new person
     */
    document.getElementById('createPersonButton').addEventListener('click', function (event) {
        event.preventDefault();

        // Gather input values from the form
        const name = document.getElementById('firstName').value.trim();
        const surname = document.getElementById('lastName').value.trim();
        const age = parseInt(document.getElementById('age').value, 10);
        const email = document.getElementById('email').value.trim();
        const address = document.getElementById('address').value.trim();
        const positionName = document.getElementById('position').value.trim();
        const departmentName = document.getElementById('department').options[document.getElementById('department').selectedIndex].text;
        const salaryAmount = parseInt(document.getElementById('salary').value, 10);
        const birthDay = document.getElementById('birthday').value;
        const city = document.getElementById('city').value.trim();

        // Validate required fields
        if (!name || !surname || isNaN(age) || !email || !address || !positionName || !departmentName || isNaN(salaryAmount) || !birthDay || !city) {
            toastr.error('All fields are required', 'Validation Error');
            return;
        }

        // Get CSRF token
        const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

        if (!csrfToken) {
            toastr.error('Failed to create person: CSRF token not found', 'Error');
            return;
        }

        // Prepare data for the create person request
        const createData = {
            name: name,
            surname: surname,
            age: age,
            email: email,
            address: address,
            position: {
                name: positionName,
                department: {
                    departmentName: departmentName
                }
            },
            salary: {
                amount: salaryAmount
            },
            personDetails: {
                birthDay: birthDay,
                personCity: city
            }
        };

        // Send create person request to the server
        fetch('/Person/CreatePerson', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': csrfToken
            },
            body: JSON.stringify(createData)
        })
            .then(response => {
                if (response.ok) {
                    toastr.success('Person created successfully');
                    document.getElementById('createPersonForm').reset();
                } else {
                    toastr.error('Failed to create person', 'Error');
                    response.text().then(text => console.error(text));
                }
            })
            .catch(error => {
                toastr.error('Error creating person', 'Error');
            });
    });

    /**
     * Handle click on the create department button
     * Validates the department name and sends a POST request to create a new department
     */
    document.getElementById('createDepartmentButton').addEventListener('click', function (event) {
        event.preventDefault();

        // Gather input value for the department name
        const departmentName = document.getElementById('departmentName').value.trim();

        // Validate the department name
        if (!departmentName) {
            toastr.error('Department name is required', 'Validation Error');
            return;
        }

        // Get CSRF token
        const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

        if (!csrfToken) {
            toastr.error('Failed to create department: CSRF token not found', 'Error');
            return;
        }

        // Send create department request to the server
        fetch('/Department/CreateDepartment', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': csrfToken
            },
            body: JSON.stringify(departmentName)
        })
            .then(response => {
                if (response.ok) {
                    toastr.success('Department created successfully');
                    document.getElementById('createDepartmentForm').reset();
                } else {
                    toastr.error('Failed to create department', 'Error');
                }
            })
            .catch(error => {
                toastr.error('Error creating department', 'Error');
            });
    });
});