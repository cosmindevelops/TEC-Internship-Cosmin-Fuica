document.addEventListener('DOMContentLoaded', function () {
    let activeEditDepartmentId = null;

    // Elements for edit, save, cancel, delete, and create actions
    const editIcons = document.querySelectorAll('.edit-icon');
    const saveIcons = document.querySelectorAll('.save-icon');
    const cancelIcons = document.querySelectorAll('.cancel-icon');
    const deleteIcons = document.querySelectorAll('.delete-icon');
    const createIcon = document.getElementById('create-department-icon');

    /**
     * Handle click on edit icons
     * This function shows the input field for the selected department and hides others
     */
    editIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const departmentId = this.getAttribute('data-department-id');

            // Hide input fields of previously active department (if any)
            if (activeEditDepartmentId && activeEditDepartmentId !== departmentId) {
                const previousDepartmentName = document.getElementById(`department-name-${activeEditDepartmentId}`);
                const previousDepartmentInput = document.getElementById(`department-input-${activeEditDepartmentId}`);
                const previousEditIcon = document.querySelector(`.edit-icon[data-department-id="${activeEditDepartmentId}"]`);
                const previousSaveIcon = document.querySelector(`.save-icon[data-department-id="${activeEditDepartmentId}"]`);
                const previousCancelIcon = document.querySelector(`.cancel-icon[data-department-id="${activeEditDepartmentId}"]`);

                previousDepartmentName.style.display = 'block';  // Show the department name
                previousDepartmentInput.style.display = 'none';  // Hide the input field
                previousEditIcon.style.display = 'inline';  // Show the edit icon
                previousSaveIcon.style.display = 'none';  // Hide the save icon
                previousCancelIcon.style.display = 'none';  // Hide the cancel icon
            }

            // Show input fields for the selected department
            const departmentName = document.getElementById(`department-name-${departmentId}`);
            const departmentInput = document.getElementById(`department-input-${departmentId}`);
            const saveIcon = document.querySelector(`.save-icon[data-department-id="${departmentId}"]`);
            const cancelIcon = document.querySelector(`.cancel-icon[data-department-id="${departmentId}"]`);

            if (departmentName && departmentInput && saveIcon && cancelIcon) {
                departmentName.style.display = 'none';  // Hide the department name
                departmentInput.style.display = 'block';  // Show the input field
                this.style.display = 'none';  // Hide the edit icon
                saveIcon.style.display = 'inline';  // Show the save icon
                cancelIcon.style.display = 'inline';  // Show the cancel icon
                activeEditDepartmentId = departmentId;  // Set the currently active department
            }
        });
    });

    /**
     * Handle click on save icons
     * This function saves the new department name and updates the server
     */
    saveIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const departmentId = this.getAttribute('data-department-id');
            const departmentInput = document.getElementById(`department-input-${departmentId}`);
            const newDepartmentName = departmentInput.value.trim();

            // Validate new department name
            if (!newDepartmentName) {
                toastr.error('Department name cannot be empty');
                return;
            }

            const csrfTokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
            const csrfToken = csrfTokenElement ? csrfTokenElement.value : '';

            // Check for CSRF token
            if (!csrfToken) {
                toastr.error('Failed to update department: CSRF token not found');
                return;
            }

            // Send update request to the server
            fetch(`/Department/UpdateDepartment?departmentId=${departmentId}&newDepartmentName=${encodeURIComponent(newDepartmentName)}`, {
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
                        toastr.error('Failed to update department');
                    }
                })
                .catch(error => {
                    toastr.error('Error updating department');
                });
        });
    });

    /**
     * Handle click on cancel icons
     * This function reverts the input field to the original department name
     */
    cancelIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const departmentId = this.getAttribute('data-department-id');

            // Revert the input field to the original department name
            const departmentName = document.getElementById(`department-name-${departmentId}`);
            const departmentInput = document.getElementById(`department-input-${departmentId}`);
            const editIcon = document.querySelector(`.edit-icon[data-department-id="${departmentId}"]`);
            const saveIcon = document.querySelector(`.save-icon[data-department-id="${departmentId}"]`);
            const cancelIcon = document.querySelector(`.cancel-icon[data-department-id="${departmentId}"]`);

            departmentName.style.display = 'block';  // Show the department name
            departmentInput.style.display = 'none';  // Hide the input field
            editIcon.style.display = 'inline';  // Show the edit icon
            saveIcon.style.display = 'none';  // Hide the save icon
            cancelIcon.style.display = 'none';  // Hide the cancel icon
            activeEditDepartmentId = null;  // Clear the currently active department
        });
    });

    /**
     * Handle click on delete icons
     * This function deletes the department from the server
     */
    deleteIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const departmentId = this.getAttribute('data-department-id');

            const csrfTokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
            const csrfToken = csrfTokenElement ? csrfTokenElement.value : '';

            // Check for CSRF token
            if (!csrfToken) {
                toastr.error('Failed to delete department: CSRF token not found');
                return;
            }

            // Send delete request to the server
            fetch(`/Department/DeleteDepartment?departmentId=${departmentId}`, {
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
                        toastr.error('Failed to delete department');
                    }
                })
                .catch(error => {
                    toastr.error('Error deleting department');
                });
        });
    });

    /**
     * Handle click on create department icon
     * This function creates a new department on the server
     */
    createIcon.addEventListener('click', function () {
        const newDepartmentName = document.getElementById('new-department-name').value.trim();

        // Validate new department name
        if (!newDepartmentName) {
            toastr.error('Department name cannot be empty');
            return;
        }

        const csrfTokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
        const csrfToken = csrfTokenElement ? csrfTokenElement.value : '';
        const token = localStorage.getItem('token');

        // Check for CSRF token
        if (!csrfToken) {
            toastr.error('Failed to create department: CSRF token not found');
            return;
        }

        // Check for authorization token
        if (!token) {
            toastr.error('Failed to create department: Authorization token not found');
            return;
        }

        // Send create request to the server
        fetch(`${apiUrl}/department`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': csrfToken,
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({ departmentName: newDepartmentName })
        })
            .then(response => {
                if (response.ok) {
                    location.reload(); // Reload the page on success
                } else {
                    toastr.error('Failed to create department');
                }
            })
            .catch(error => {
                toastr.error('Error creating department');
            });
    });
});