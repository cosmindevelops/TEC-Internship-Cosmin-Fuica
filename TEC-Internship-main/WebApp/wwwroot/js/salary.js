document.addEventListener('DOMContentLoaded', function () {
    let activeEditPersonId = null;

    const editIcons = document.querySelectorAll('.edit-icon');
    const saveIcons = document.querySelectorAll('.save-icon');
    const cancelIcons = document.querySelectorAll('.cancel-icon');

    const MAX_INT_VALUE = 2147483647;

    editIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const personId = this.getAttribute('data-person-id');

            if (activeEditPersonId && activeEditPersonId !== personId) {
                const previousSalaryAmount = document.getElementById(`salary-amount-${activeEditPersonId}`);
                const previousSalaryInput = document.getElementById(`salary-input-${activeEditPersonId}`);
                const previousEditIcon = document.querySelector(`.edit-icon[data-person-id="${activeEditPersonId}"]`);
                const previousSaveIcon = document.querySelector(`.save-icon[data-person-id="${activeEditPersonId}"]`);
                const previousCancelIcon = document.querySelector(`.cancel-icon[data-person-id="${activeEditPersonId}"]`);

                previousSalaryAmount.style.display = 'block';
                previousSalaryInput.style.display = 'none';
                previousEditIcon.style.display = 'inline';
                previousSaveIcon.style.display = 'none';
                previousCancelIcon.style.display = 'none';
            }

            const salaryAmount = document.getElementById(`salary-amount-${personId}`);
            const salaryInput = document.getElementById(`salary-input-${personId}`);
            const saveIcon = document.querySelector(`.save-icon[data-person-id="${personId}"]`);
            const cancelIcon = document.querySelector(`.cancel-icon[data-person-id="${personId}"]`);

            if (salaryAmount && salaryInput && saveIcon && cancelIcon) {
                salaryAmount.style.display = 'none';
                salaryInput.style.display = 'block';
                this.style.display = 'none';
                saveIcon.style.display = 'inline';
                cancelIcon.style.display = 'inline';
                activeEditPersonId = personId;
            } else {
                toastr.error(`Elements not found for person ID ${personId}`, 'Error');
            }
        });
    });

    saveIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const personId = this.getAttribute('data-person-id');
            const salaryInput = document.getElementById(`salary-input-${personId}`);

            if (!salaryInput) {
                toastr.error(`Could not find salary input for person ID ${personId}`, 'Error');
                return;
            }

            const newAmount = parseInt(salaryInput.value, 10);

            if (isNaN(newAmount)) {
                toastr.error('Salary must be a number.', 'Validation Error');
                return;
            }

            if (newAmount <= 0) {
                toastr.error('Salary must be greater than 0.', 'Validation Error');
                return;
            }

            if (newAmount > MAX_INT_VALUE) {
                toastr.error(`Salary must be less than or equal to ${MAX_INT_VALUE}.`, 'Validation Error');
                return;
            }

            const csrfTokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
            const csrfToken = csrfTokenElement ? csrfTokenElement.value : '';

            if (!csrfToken) {
                toastr.error('Failed to update salary: CSRF token not found', 'Error');
                return;
            }

            fetch(`/Salary/UpdateSalary?personId=${personId}&newSalaryAmount=${newAmount}`, {
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
                        toastr.error('Failed to update salary', 'Error');
                    }
                })
                .catch(error => {
                    toastr.error('Error updating salary', 'Error');
                });
        });
    });

    cancelIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const personId = this.getAttribute('data-person-id');
            const salaryAmount = document.getElementById(`salary-amount-${personId}`);
            const salaryInput = document.getElementById(`salary-input-${personId}`);
            const editIcon = document.querySelector(`.edit-icon[data-person-id="${personId}"]`);
            const saveIcon = document.querySelector(`.save-icon[data-person-id="${personId}"]`);
            const cancelIcon = this;

            if (salaryAmount && salaryInput && editIcon && saveIcon && cancelIcon) {
                salaryAmount.style.display = 'block';
                salaryInput.style.display = 'none';
                editIcon.style.display = 'inline';
                saveIcon.style.display = 'none';
                cancelIcon.style.display = 'none';
                activeEditPersonId = null;
            } else {
                toastr.error(`Elements not found for person ID ${personId}`, 'Error');
            }
        });
    });
});