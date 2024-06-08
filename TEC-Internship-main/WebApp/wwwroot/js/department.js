document.addEventListener('DOMContentLoaded', function () {
    let activeEditDepartmentId = null;

    const editIcons = document.querySelectorAll('.edit-icon');
    const saveIcons = document.querySelectorAll('.save-icon');
    const cancelIcons = document.querySelectorAll('.cancel-icon');
    const deleteIcons = document.querySelectorAll('.delete-icon');
    const createIcon = document.getElementById('create-department-icon');

    editIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const departmentId = this.getAttribute('data-department-id');

            if (activeEditDepartmentId && activeEditDepartmentId !== departmentId) {
                const previousDepartmentName = document.getElementById(`department-name-${activeEditDepartmentId}`);
                const previousDepartmentInput = document.getElementById(`department-input-${activeEditDepartmentId}`);
                const previousEditIcon = document.querySelector(`.edit-icon[data-department-id="${activeEditDepartmentId}"]`);
                const previousSaveIcon = document.querySelector(`.save-icon[data-department-id="${activeEditDepartmentId}"]`);
                const previousCancelIcon = document.querySelector(`.cancel-icon[data-department-id="${activeEditDepartmentId}"]`);

                previousDepartmentName.style.display = 'block';
                previousDepartmentInput.style.display = 'none';
                previousEditIcon.style.display = 'inline';
                previousSaveIcon.style.display = 'none';
                previousCancelIcon.style.display = 'none';
            }

            const departmentName = document.getElementById(`department-name-${departmentId}`);
            const departmentInput = document.getElementById(`department-input-${departmentId}`);
            const saveIcon = document.querySelector(`.save-icon[data-department-id="${departmentId}"]`);
            const cancelIcon = document.querySelector(`.cancel-icon[data-department-id="${departmentId}"]`);

            if (departmentName && departmentInput && saveIcon && cancelIcon) {
                departmentName.style.display = 'none';
                departmentInput.style.display = 'block';
                this.style.display = 'none';
                saveIcon.style.display = 'inline';
                cancelIcon.style.display = 'inline';
                activeEditDepartmentId = departmentId;
            }
        });
    });

    saveIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const departmentId = this.getAttribute('data-department-id');
            const departmentInput = document.getElementById(`department-input-${departmentId}`);
            const newDepartmentName = departmentInput.value.trim();

            if (!newDepartmentName) {
                toastr.error('Department name cannot be empty');
                return;
            }

            const csrfTokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
            const csrfToken = csrfTokenElement ? csrfTokenElement.value : '';
            const token = localStorage.getItem('token');

            if (!csrfToken) {
                toastr.error('Failed to update department: CSRF token not found');
                return;
            }

            if (!token) {
                toastr.error('Failed to create person: Authorization token not found', 'Error');
                console.error('Authorization token not found');
                return;
            }

            fetch(`/Department/UpdateDepartment?departmentId=${departmentId}&newDepartmentName=${encodeURIComponent(newDepartmentName)}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': csrfToken,
                    'Authorization': `Bearer ${token}`
                }
            })
                .then(response => {
                    if (response.ok) {
                        location.reload();
                    } else {
                        toastr.error('Failed to update department');
                    }
                })
                .catch(error => {
                    toastr.error('Error updating department');
                });
        });
    });

    cancelIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const departmentId = this.getAttribute('data-department-id');

            const departmentName = document.getElementById(`department-name-${departmentId}`);
            const departmentInput = document.getElementById(`department-input-${departmentId}`);
            const editIcon = document.querySelector(`.edit-icon[data-department-id="${departmentId}"]`);
            const saveIcon = document.querySelector(`.save-icon[data-department-id="${departmentId}"]`);
            const cancelIcon = document.querySelector(`.cancel-icon[data-department-id="${departmentId}"]`);

            departmentName.style.display = 'block';
            departmentInput.style.display = 'none';
            editIcon.style.display = 'inline';
            saveIcon.style.display = 'none';
            cancelIcon.style.display = 'none';
            activeEditDepartmentId = null;
        });
    });

    deleteIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const departmentId = this.getAttribute('data-department-id');

            const csrfTokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
            const csrfToken = csrfTokenElement ? csrfTokenElement.value : '';
            const token = localStorage.getItem('token');

            if (!csrfToken) {
                toastr.error('Failed to delete department: CSRF token not found');
                return;
            }

            if (!token) {
                toastr.error('Failed to create person: Authorization token not found', 'Error');
                console.error('Authorization token not found');
                return;
            }

            fetch(`/Department/DeleteDepartment?departmentId=${departmentId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': csrfToken,
                    'Authorization': `Bearer ${token}`
                }
            })
                .then(response => {
                    if (response.ok) {
                        location.reload();
                    } else {
                        toastr.error('Failed to delete department');
                    }
                })
                .catch(error => {
                    toastr.error('Error deleting department');
                });
        });
    });

    createIcon.addEventListener('click', function () {
        const newDepartmentName = document.getElementById('new-department-name').value.trim();

        if (!newDepartmentName) {
            toastr.error('Department name cannot be empty');
            return;
        }

        const csrfTokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
        const csrfToken = csrfTokenElement ? csrfTokenElement.value : '';
        const token = localStorage.getItem('token');

        if (!csrfToken) {
            toastr.error('Failed to create department: CSRF token not found');
            return;
        }

        if (!token) {
            toastr.error('Failed to create person: Authorization token not found', 'Error');
            return;
        }
        
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
                    location.reload();
                } else {
                    toastr.error('Failed to create department');
                }
            })
            .catch(error => {
                toastr.error('Error creating department');
            });
    });
});