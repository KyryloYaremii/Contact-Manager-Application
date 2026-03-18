
document.addEventListener('DOMContentLoaded', () => {

    const tableBody = document.getElementById('employeesTableBody');
    const uploadBtn = document.getElementById('uploadBtn');
    const csvFileInput = document.getElementById('csvFileInput');
    const uploadMessage = document.getElementById('uploadMessage');

    let employeesData = [];

    async function loadEmployees() {
        try {
            const response = await fetch('/api/employees');

            if (!response.ok) throw new Error('Network error');

            employeesData = await response.json();
            renderTable(employeesData);

        } catch (error) {
            console.error('Loading Error:', error);
            tableBody.innerHTML = '<tr><td colspan="6" class="text-danger text-center">Error loading data</td></tr>';
        }
    }

    async function uploadCsv() {
        const file = csvFileInput.files[0];
        if (!file) {
            alert("Please, choose the file!");
            return;
        }

        const formData = new FormData();
        formData.append('file', file);

        try {
            uploadBtn.disabled = true; 
            uploadBtn.innerHTML = 'Uploading...';

            const response = await fetch('/api/employees/upload', {
                method: 'POST',
                body: formData
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Error due file uploading');
            }

            uploadMessage.textContent = 'File successfully processed!';
            uploadMessage.style.display = 'block';
            csvFileInput.value = ''; 

            await loadEmployees();

        } catch (error) {
            console.error('Upload Error:', error);
            alert('Error: ' + error.message);
        } finally {
            uploadBtn.disabled = false;
            uploadBtn.innerHTML = 'Upload';
        }
    }

    uploadBtn.addEventListener('click', uploadCsv);

    function renderTable(data) {
        tableBody.innerHTML = '';

        if (data.length === 0) {
            tableBody.innerHTML = '<tr><td colspan="6" class="text-center">No data. Upload a CSV file..</td></tr>';
            return;
        }

        data.forEach(emp => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${emp.name}</td>
                <td>${new Date(emp.dateOfBirth).toLocaleDateString()}</td>
                <td>${emp.married ? 'Yes' : 'No'}</td>
                <td>${emp.phone || ''}</td>
                <td>${emp.salary}</td>
                <td>
                    <button class="btn btn-sm btn-outline-primary edit-btn" data-id="${emp.id}">✏️</button>
                    <button class="btn btn-sm btn-outline-danger delete-btn" data-id="${emp.id}">🗑️</button>
                </td>
            `;
            tableBody.appendChild(tr);
        });
    }

    document.getElementById('searchInput').addEventListener('input', (e) => {
        const searchTerm = e.target.value.toLowerCase();

        const filteredData = employeesData.filter(emp => {
            const formattedDate = new Date(emp.dateOfBirth).toLocaleDateString().toLowerCase();

            const marriedStatus = emp.married ? 'yes' : 'no';

            return (
                emp.name.toLowerCase().includes(searchTerm) ||
                formattedDate.includes(searchTerm) ||
                marriedStatus.includes(searchTerm) ||
                (emp.phone && emp.phone.toLowerCase().includes(searchTerm)) ||
                emp.salary.toString().includes(searchTerm)
            );
        });

        renderTable(filteredData);
    });

    const editModal = new bootstrap.Modal(document.getElementById('editModal'));

    document.getElementById('saveEditBtn').addEventListener('click', async () => {
        const id = document.getElementById('editId').value;
        const name = document.getElementById('editName').value.trim();
        const dateOfBirth = document.getElementById('editDate').value;
        const phone = document.getElementById('editPhone').value.trim();
        const salary = parseFloat(document.getElementById('editSalary').value);
        const married = document.getElementById('editMarried').checked;

        let errors = [];

        if (!name) errors.push("The name can't be empty.");
        if (!dateOfBirth) errors.push("Birth date is required.");
        else {
            const birthDate = new Date(dateOfBirth);
            if (birthDate > new Date()) errors.push("Birth date cannot be in the future time.");
        }
        if (isNaN(salary) || salary < 0) errors.push("Salary must be a number greater than or equal to 0.");
        if (phone && !/^\+?[\d\s\-()]{7,20}$/.test(phone)) {
            errors.push("Incorrect phone format.");
        }

        if (errors.length > 0) {
            alert("Filling error:\n- " + errors.join("\n- "));
            return; 
        }

        const dto = { name, dateOfBirth, married, phone, salary };

        try {
            const response = await fetch(`/api/employees/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(dto)
            });

            if (response.ok) {
                bootstrap.Modal.getInstance(document.getElementById('editModal')).hide();
                await loadEmployees();
            } else {
                const errData = await response.json();
                alert(errData.message || 'Error during update');
            }
        } catch (err) {
            console.error('Update error:', err);
        }
    });

    function editEmployee(id) {
        const emp = employeesData.find(e => e.id == id);
        if (!emp) return;

        document.getElementById('editId').value = emp.id;
        document.getElementById('editName').value = emp.name;

        if (emp.dateOfBirth) {
            document.getElementById('editDate').value = emp.dateOfBirth.split('T')[0];
        }

        document.getElementById('editMarried').checked = emp.married;
        document.getElementById('editPhone').value = emp.phone || '';
        document.getElementById('editSalary').value = emp.salary;

        editModal.show();
    }

    tableBody.addEventListener('click', async (e) => {
        const id = e.target.closest('button')?.dataset.id;
        if (!id) return;

        if (e.target.closest('.delete-btn')) {
            if (!confirm('Are you sure that you want to delete it?')) return;

            try {
                const response = await fetch(`/api/employees/${id}`, { method: 'DELETE' });
                if (response.ok) {
                    await loadEmployees(); 
                } else {
                    alert('Error occured');
                }
            } catch (err) {
                console.error(err);
            }
        }

        if (e.target.closest('.edit-btn')) {
            editEmployee(id);
        }
    });

    let sortDirection = true;

    document.querySelectorAll('th[data-sort]').forEach(header => {
        header.addEventListener('click', () => {
            const field = header.dataset.sort;
            sortDirection = !sortDirection;

            const sorted = [...employeesData].sort((a, b) => {
                let valA = a[field];
                let valB = b[field];

                if (typeof valA === 'string') {
                    return sortDirection
                        ? valA.localeCompare(valB)
                        : valB.localeCompare(valA);
                }
                return sortDirection ? valA - valB : valB - valA;
            });

            renderTable(sorted);
        });
    });

    loadEmployees();
});