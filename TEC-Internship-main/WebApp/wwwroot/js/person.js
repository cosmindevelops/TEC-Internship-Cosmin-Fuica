document.addEventListener('DOMContentLoaded', function () {
    let activeEditPersonId = null;

    const editIcons = document.querySelectorAll('.edit-icon');
    const saveIcons = document.querySelectorAll('.save-icon');
    const cancelIcons = document.querySelectorAll('.cancel-icon');
    const deleteIcons = document.querySelectorAll('.delete-icon');

    const columnCheckboxes = document.querySelectorAll('.column-visibility-controls input[type="checkbox"]');

    columnCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function () {
            const column = this.getAttribute('data-column');
            const table = document.querySelector('.custom-table');
            const rows = table.querySelectorAll('tr');

            rows.forEach(row => {
                const cells = row.querySelectorAll('th, td');
                if (cells[column]) {
                    cells[column].style.display = this.checked ? '' : 'none';
                }
            });
        });
    });

    function cancelEdit(personRow) {
        personRow.querySelectorAll('span').forEach(span => span.style.display = 'block');
        personRow.querySelectorAll('input').forEach(input => input.style.display = 'none');
        personRow.querySelector('.edit-icon').style.display = 'inline';
        personRow.querySelector('.save-icon').style.display = 'none';
        personRow.querySelector('.cancel-icon').style.display = 'none';
        activeEditPersonId = null;
    }

    cancelIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const personId = this.getAttribute('data-person-id');
            const personRow = this.closest('tr');
            cancelEdit(personRow);
        });
    });

    editIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const personId = this.getAttribute('data-person-id');
            const personRow = this.closest('tr');

            if (activeEditPersonId && activeEditPersonId !== personId) {
                const previousRow = document.querySelector(`tr[data-person-id="${activeEditPersonId}"]`);
                cancelEdit(previousRow);
            }

            personRow.querySelectorAll('span').forEach(span => span.style.display = 'none');
            personRow.querySelectorAll('input').forEach(input => input.style.display = 'block');
            personRow.querySelector('.edit-icon').style.display = 'none';
            personRow.querySelector('.save-icon').style.display = 'inline';
            personRow.querySelector('.cancel-icon').style.display = 'inline';
            activeEditPersonId = personId;
        });
    });

    saveIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const personId = this.getAttribute('data-person-id');
            const personRow = this.closest('tr');
            const name = personRow.querySelector('.name-input').value.trim();
            const surname = personRow.querySelector('.surname-input').value.trim();
            const age = parseInt(personRow.querySelector('.age-input').value, 10);
            const email = personRow.querySelector('.email-input').value.trim();
            const address = personRow.querySelector('.address-input').value.trim();
            const positionName = personRow.querySelector('.position-input').value.trim();
            const departmentName = personRow.querySelector('.department-input').value.trim();
            const salaryAmount = parseInt(personRow.querySelector('.salary-input').value, 10);
            const birthDay = personRow.querySelector('.birthday-input').value;
            const personCity = personRow.querySelector('.personcity-input').value.trim();

            if (!name || !surname || isNaN(age) || !email || !address || !positionName || !departmentName || isNaN(salaryAmount) || !birthDay || !personCity) {
                toastr.error('All fields are required', 'Validation Error');
                return;
            }

            const csrfTokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
            const csrfToken = csrfTokenElement ? csrfTokenElement.value : '';

            if (!csrfToken) {
                toastr.error('Failed to update person: CSRF token not found', 'Error');
                console.error('CSRF token not found');
                return;
            }

            const updateData = {
                Name: name,
                Surname: surname,
                Age: age,
                Email: email,
                Address: address,
                Position: {
                    Name: positionName,
                    Department: {
                        DepartmentName: departmentName
                    }
                },
                Salary: {
                    Amount: salaryAmount
                },
                PersonDetails: {
                    BirthDay: birthDay,
                    PersonCity: personCity
                }
            };

            fetch(`/Person/UpdatePerson?id=${personId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': csrfToken
                },
                body: JSON.stringify(updateData)
            })
                .then(response => {
                    if (response.ok) {
                        location.reload();
                    } else {
                        toastr.error('Failed to update person', 'Error');
                    }
                })
                .catch(error => {
                    toastr.error('Error updating person', 'Error');
                    console.error('Error updating person:', error);
                });
        });
    });

    deleteIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const personId = this.getAttribute('data-person-id');
            const csrfTokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
            const csrfToken = csrfTokenElement ? csrfTokenElement.value : '';

            if (!csrfToken) {
                toastr.error('Failed to delete person: CSRF token not found', 'Error');
                console.error('CSRF token not found');
                return;
            }

            fetch(`/Person/DeletePerson?id=${personId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': csrfToken
                }
            })
                .then(response => {
                    if (response.ok) {
                        location.reload();
                    } else {
                        toastr.error('Failed to delete person', 'Error');
                    }
                })
                .catch(error => {
                    toastr.error('Error deleting person', 'Error');
                });
        });
    });
});