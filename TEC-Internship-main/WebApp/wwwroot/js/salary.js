document.addEventListener('DOMContentLoaded', function () {
    let activeEditPersonId = null;

    // Elements for edit, save, and cancel actions
    const editIcons = document.querySelectorAll('.edit-icon');
    const saveIcons = document.querySelectorAll('.save-icon');
    const cancelIcons = document.querySelectorAll('.cancel-icon');

    const MAX_INT_VALUE = 2147483647; // Maximum integer value for validation

    /**
     * Handle click on edit icons
     * Switches the row to edit mode, allowing the user to update the salary
     */
    editIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const personId = this.getAttribute('data-person-id');

            // Hide input fields of previously active person (if any)
            if (activeEditPersonId && activeEditPersonId !== personId) {
                const previousSalaryAmount = document.getElementById(`salary-amount-${activeEditPersonId}`);
                const previousSalaryInput = document.getElementById(`salary-input-${activeEditPersonId}`);
                const previousEditIcon = document.querySelector(`.edit-icon[data-person-id="${activeEditPersonId}"]`);
                const previousSaveIcon = document.querySelector(`.save-icon[data-person-id="${activeEditPersonId}"]`);
                const previousCancelIcon = document.querySelector(`.cancel-icon[data-person-id="${activeEditPersonId}"]`);

                previousSalaryAmount.style.display = 'block';  // Show the salary amount
                previousSalaryInput.style.display = 'none';  // Hide the input field
                previousEditIcon.style.display = 'inline';  // Show the edit icon
                previousSaveIcon.style.display = 'none';  // Hide the save icon
                previousCancelIcon.style.display = 'none';  // Hide the cancel icon
            }

            // Show input fields for the selected person
            const salaryAmount = document.getElementById(`salary-amount-${personId}`);
            const salaryInput = document.getElementById(`salary-input-${personId}`);
            const saveIcon = document.querySelector(`.save-icon[data-person-id="${personId}"]`);
            const cancelIcon = document.querySelector(`.cancel-icon[data-person-id="${personId}"]`);

            if (salaryAmount && salaryInput && saveIcon && cancelIcon) {
                salaryAmount.style.display = 'none';  // Hide the salary amount
                salaryInput.style.display = 'block';  // Show the input field
                this.style.display = 'none';  // Hide the edit icon
                saveIcon.style.display = 'inline';  // Show the save icon
                cancelIcon.style.display = 'inline';  // Show the cancel icon
                activeEditPersonId = personId;  // Set the currently active person ID
            } else {
                toastr.error(`Elements not found for person ID ${personId}`, 'Error');
            }
        });
    });

    /**
     * Handle click on save icons
     * Saves the updated salary amount to the server
     */
    saveIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const personId = this.getAttribute('data-person-id');
            const salaryInput = document.getElementById(`salary-input-${personId}`);

            if (!salaryInput) {
                toastr.error(`Could not find salary input for person ID ${personId}`, 'Error');
                return;
            }

            // Parse and validate the new salary amount
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

            // Get CSRF token
            const csrfTokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
            const csrfToken = csrfTokenElement ? csrfTokenElement.value : '';

            if (!csrfToken) {
                toastr.error('Failed to update salary: CSRF token not found', 'Error');
                return;
            }

            // Send update request to the server
            fetch(`/Salary/UpdateSalary?personId=${personId}&newSalaryAmount=${newAmount}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': csrfToken
                }
            })
                .then(response => {
                    if (response.ok) {
                        location.reload(); // Reload the page on success
                    } else {
                        toastr.error('Failed to update salary', 'Error');
                    }
                })
                .catch(error => {
                    toastr.error('Error updating salary', 'Error');
                });
        });
    });

    /**
     * Handle click on cancel icons
     * Reverts the row to display mode without saving changes
     */
    cancelIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const personId = this.getAttribute('data-person-id');
            const salaryAmount = document.getElementById(`salary-amount-${personId}`);
            const salaryInput = document.getElementById(`salary-input-${personId}`);
            const editIcon = document.querySelector(`.edit-icon[data-person-id="${personId}"]`);
            const saveIcon = document.querySelector(`.save-icon[data-person-id="${personId}"]`);
            const cancelIcon = this;

            if (salaryAmount && salaryInput && editIcon && saveIcon && cancelIcon) {
                salaryAmount.style.display = 'block';  // Show the salary amount
                salaryInput.style.display = 'none';  // Hide the input field
                editIcon.style.display = 'inline';  // Show the edit icon
                saveIcon.style.display = 'none';  // Hide the save icon
                cancelIcon.style.display = 'none';  // Hide the cancel icon
                activeEditPersonId = null;  // Clear the currently active person ID
            } else {
                toastr.error(`Elements not found for person ID ${personId}`, 'Error');
            }
        });
    });
});