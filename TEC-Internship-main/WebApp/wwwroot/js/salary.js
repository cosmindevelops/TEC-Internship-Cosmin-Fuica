document.addEventListener('DOMContentLoaded', function () {
    let activeEditPersonId = null;

    const editIcons = document.querySelectorAll('.edit-icon');
    const saveIcons = document.querySelectorAll('.save-icon');

    editIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const personId = this.getAttribute('data-person-id');

            // Cancel the active edit if another row is being edited
            if (activeEditPersonId && activeEditPersonId !== personId) {
                const previousSalaryAmount = document.getElementById(`salary-amount-${activeEditPersonId}`);
                const previousSalaryInput = document.getElementById(`salary-input-${activeEditPersonId}`);
                const previousEditIcon = document.querySelector(`.edit-icon[data-person-id="${activeEditPersonId}"]`);
                const previousSaveIcon = document.querySelector(`.save-icon[data-person-id="${activeEditPersonId}"]`);

                previousSalaryAmount.style.display = 'block';
                previousSalaryInput.style.display = 'none';
                previousEditIcon.style.display = 'inline';
                previousSaveIcon.style.display = 'none';
            }

            const salaryAmount = document.getElementById(`salary-amount-${personId}`);
            const salaryInput = document.getElementById(`salary-input-${personId}`);
            const saveIcon = document.querySelector(`.save-icon[data-person-id="${personId}"]`);

            console.log(`Edit - Person ID: ${personId}`);
            console.log(`Salary Amount Element:`, salaryAmount);
            console.log(`Salary Input Element:`, salaryInput);

            if (salaryAmount && salaryInput && saveIcon) {
                salaryAmount.style.display = 'none';
                salaryInput.style.display = 'block';
                this.style.display = 'none';
                saveIcon.style.display = 'inline';
                activeEditPersonId = personId; // Set the active edit person ID
            } else {
                console.error(`Elements not found for person ID ${personId}`);
            }
        });
    });

    saveIcons.forEach(icon => {
        icon.addEventListener('click', function () {
            const personId = this.getAttribute('data-person-id');
            const salaryInput = document.getElementById(`salary-input-${personId}`);

            console.log(`Save - Person ID: ${personId}`);
            console.log(`Salary Input Element:`, salaryInput);

            if (!salaryInput) {
                console.error(`Could not find salary input for person ID ${personId}`);
                return;
            }

            const newAmount = parseInt(salaryInput.value, 10);
            console.log(`New Amount: ${newAmount}`);

            const csrfTokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
            const csrfToken = csrfTokenElement ? csrfTokenElement.value : '';

            if (!csrfToken) {
                console.error('CSRF token not found');
                alert('Failed to update salary: CSRF token not found');
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
                        alert('Failed to update salary');
                    }
                })
                .catch(error => {
                    console.error('Error updating salary:', error);
                    alert('Failed to update salary');
                });
        });
    });
});
