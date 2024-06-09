# API Documentation

## Table of Contents

1. [Overview](#overview)
2. [Base URLs](#base-urls)
3. [Authentication Endpoints](#authentication-endpoints)
   - [Register](#register)
   - [Login](#login)
4. [Person Endpoints](#person-endpoints)
   - [Get All Persons](#get-all-persons)
   - [Get Person by ID](#get-person-by-id)
   - [Get Total Persons](#get-total-persons)
   - [Create Person](#create-person)
   - [Update Person](#update-person)
   - [Delete Person](#delete-person)
5. [Department Endpoints](#department-endpoints)
   - [Get All Departments](#get-all-departments)
   - [Get Department by ID](#get-department-by-id)
   - [Get Total Departments](#get-total-departments)
   - [Create Department](#create-department)
   - [Delete Department](#delete-department)
   - [Change Person Department](#change-person-department)
   - [Update Department Name](#update-department-name)
6. [Salary Endpoints](#salary-endpoints)
   - [Get All Salaries](#get-all-salaries)
   - [Get Salary by Person ID](#get-salary-by-person-id)
   - [Delete Salary](#delete-salary)
   - [Update Salary](#update-salary)
7. [DTOs](#dtos)
   - [LoginModelDto](#loginmodeldto)
   - [RegisterModelDto](#registermodeldto)
   - [AuthResponseDto](#authresponsedto)
   - [PersonDto](#persondto)
   - [PersonDetailsDto](#persondetailsdto)
   - [CreateUpdatePersonDto](#createupdatepersondto)
   - [CreateUpdatePersonDetailsDto](#createupdatepersondetailsdto)
   - [DepartmentDto](#departmentdto)
   - [CreateUpdateDepartmentDto](#createupdatedepartmentdto)
   - [SalaryDto](#salarydto)
   - [CreateUpdateSalaryDto](#createupdatesalarydto)
   - [SalaryWithFullNameDto](#salarywithfullnamedto)
   - [PositionDto](#positiondto)
   - [CreateUpdatePositionDto](#createupdatepositiondto)
8. [Error Handling](#error-handling)
   - [User Already Exists](#user-already-exists)
   - [Invalid Credentials](#invalid-credentials)
   - [Person Not Found](#person-not-found)
   - [Department Not Found](#department-not-found)
   - [Salary Not Found](#salary-not-found)
9. [Additional Information](#additional-information)

## Overview

Provides endpoints for user registration, authentication, managing persons, departments, and salaries.

## Base URLs

- **API Application:**
  - Development: `http://localhost:6070`
- **Web Application:**
  - Development: `https://localhost:44315`

## Authentication Endpoints

### Register

**Endpoint:** `POST /api/auth/register`

**Description:** Registers a new user.

**Request Body:**

- `username`: Required, string, 3-50 characters.
- `email`: Required, string, valid email format.
- `password`: Required, string, 6-100 characters.

**Example Request:**

```json
{
  "username": "john_doe",
  "email": "john.doe@example.com",
  "password": "Password123"
}
```

**Example Response:**

```json
{
  "message": "User registered successfully."
}
```

### Login

**Endpoint:** `POST /api/auth/login`

**Description:** Authenticates a user and returns a JWT token.

**Request Body:**

- `email`: Required, string, valid email format.
- `password`: Required, string, 6-100 characters.

**Example Request:**

```json
{
  "email": "john.doe@example.com",
  "password": "Password123"
}
```

**Example Response:**

```json
{
  "userId": "123e4567-e89b-12d3-a456-426614174000",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "john_doe"
}
```

## Person Endpoints

### Get All Persons

**Endpoint:** `GET /api/person`

**Description:** Retrieves all persons.

**Example Response:**

```json
[
  {
    "id": 1,
    "name": "John",
    "surname": "Doe",
    "age": 30,
    "email": "john.doe@example.com",
    "address": "123 Main St",
    "position": {
      "name": "Developer",
      "department": {
        "departmentName": "IT"
      }
    },
    "salary": {
      "amount": 50000
    },
    "personDetails": {
      "birthDay": "1990-01-01",
      "personCity": "New York"
    }
  }
]
```

### Get Person by ID

**Endpoint:** `GET /api/person/{id}`

**Description:** Retrieves a person by their ID.

**Example Response:**

```json
{
  "id": 1,
  "name": "John",
  "surname": "Doe",
  "age": 30,
  "email": "john.doe@example.com",
  "address": "123 Main St",
  "position": {
    "name": "Developer",
    "department": {
      "departmentName": "IT"
    }
  },
  "salary": {
    "amount": 50000
  },
  "personDetails": {
    "birthDay": "1990-01-01",
    "personCity": "New York"
  }
}
```

### Get Total Persons

**Endpoint:** `GET /api/person/total`

**Description:** Retrieves the total number of persons.

**Example Response:**

```json
{
  "totalPersons": 10
}
```

### Create Person

**Endpoint:** `POST /api/person`

**Description:** Creates a new person.

**Request Body:**

- `name`: Required, string, 1-50 characters.
- `surname`: Required, string, 1-50 characters.
- `age`: Required, integer, 1-120.
- `email`: Required, string, valid email format.
- `address`: Required, string, 1-100 characters.
- `personDetails`: Optional, object.
- `position`: Optional, object.
- `salary`: Optional, object.

**Example Request:**

```json
{
  "name": "John",
  "surname": "Doe",
  "age": 30,
  "email": "john.doe@example.com",
  "address": "123 Main St",
  "position": {
    "name": "Developer",
    "department": {
      "departmentName": "IT"
    }
  },
  "salary": {
    "amount": 50000
  },
  "personDetails": {
    "birthDay": "1990-01-01",
    "personCity": "New York"
  }
}
```

**Example Response:**

```json
{
  "id": 1,
  "name": "John",
  "surname": "Doe",
  "age": 30,
  "email": "john.doe@example.com",
  "address": "123 Main St",
  "position": {
    "name": "Developer",
    "department": {
      "departmentName": "IT"
    }
  },
  "salary": {
    "amount": 50000
  },
  "personDetails": {
    "birthDay": "1990-01-01",
    "personCity": "New York"
  }
}
```

### Update Person

**Endpoint:** `PUT /api/person/{id}`

**Description:** Updates a person's information.

**Request Body:**

- `name`: Required, string, 1-50 characters.
- `surname`: Required, string, 1-50 characters.
- `age`: Required, integer, 1-120.
- `email`: Required, string, valid email format.
- `address`: Required, string, 1-100 characters.
- `personDetails`: Optional, object.
- `position`: Optional, object.
- `salary`: Optional, object.

**Example Request:**

```json
{
  "name": "John",
  "surname": "Doe",
  "age": 31,
  "email": "john.doe@example.com",
  "address": "456 Main St",
  "position": {
    "name": "Senior Developer",
    "department": {
      "departmentName": "IT"
    }
  },
  "salary": {
    "amount": 60000
  },
  "personDetails": {
    "birthDay": "1990-01-01",
    "personCity": "New York"
  }
}
```

**Example Response:**

```json
{
  "message": "Person updated successfully."
}
```

### Delete Person

**Endpoint:** `DELETE /api/person/{id}`

**Description:** Deletes a person.

**Example Response:**

```json
{
  "message": "Person deleted successfully."
}
```

## Department Endpoints

### Get All Departments

**Endpoint:** `GET /api/department`

**Description:** Retrieves all departments except "Unassigned".

**Example Response:**

```json
[
  {
    "departmentId": 1,
    "departmentName": "IT"
  },
  {
    "departmentId": 2,
    "departmentName": "HR"
 }
]
```

### Get Department by ID

**Endpoint:** `GET /api/department/{id}`

**Description:** Retrieves a department by its ID.

**Example Response:**

```json
{
  "departmentId": 1,
  "departmentName": "IT"
}
```

### Get Total Departments

**Endpoint:** `GET /api/department/total`

**Description:** Retrieves the total number of departments excluding "Unassigned".

**Example Response:**

```json
{
  "totalDepartments": 5
}
```

### Create Department

**Endpoint:** `POST /api/department`

**Description:** Creates a new department.

**Request Body:**

- `departmentName`: Required, string, up to 50 characters.

**Example Request:**

```json
{
  "departmentName": "Finance"
}
```

**Example Response:**

```json
{
  "departmentId": 3,
  "departmentName": "Finance"
}
```

### Delete Department

**Endpoint:** `DELETE /api/department/{id}`

**Description:** Deletes a department.

**Example Response:**

```json
{
  "message": "Department deleted successfully."
}
```


### Change Person Department

**Endpoint:** `PUT /api/department/ChangePersonDepartment`

**Description:** Changes the department of a person.

**Request Parameters:**

- `personId`: Required, integer.
- `newDepartmentName`: Required, string.

**Example Request:**

```
PUT /api/department/ChangePersonDepartment?personId=1&newDepartmentName=Marketing
```

**Example Response:**

```json
{
  "message": "Person department changed successfully."
}
```

### Update Department Name

**Endpoint:** `PUT /api/department/{id}`

**Description:** Updates a department's name.

**Request Body:**

- `departmentName`: Required, string, up to 50 characters.

**Example Request:**

```json
{
  "departmentName": "New Department Name"
}
```

**Example Response:**

```json
{
  "message": "Department name updated successfully."
}
```

## Salary Endpoints

### Get All Salaries

**Endpoint:** `GET /api/salary`

**Description:** Retrieves all salaries with person's full name.

**Example Response:**

```json
[
  {
    "personId": 1,
    "fullName": "John Doe",
    "amount": 50000
  },
  {
    "personId": 2,
    "fullName": "Jane Smith",
    "amount": 60000
  }
]
```

### Get Salary by Person ID

**Endpoint:** `GET /api/salary/{personId}`

**Description:** Retrieves a salary by person ID.

**Example Response:**

```json
{
  "salaryId": 1,
  "amount": 50000
}
```

### Delete Salary

**Endpoint:** `PUT /api/salary/{personId}`

**Description:** Deletes a salary.

**Example Response:**

```json
{
  "message": "Salary deleted successfully."
}
```


### Update Salary

**Endpoint:** `PUT /api/salary/UpdateSalary`

**Description:** Updates a person's salary.

**Request Parameters:**

- `personId`: Required, integer.
- `newSalaryAmount`: Required, integer, greater than 0.

**Example Request:**

```
PUT /api/salary/UpdateSalary?personId=1&newSalaryAmount=70000
```

**Example Response:**

```json
{
  "message": "Salary updated successfully."
}
```

## DTOs

### LoginModelDto

- `email`: Required, string, valid email format.
- `password`: Required, string, 6-100 characters.

### RegisterModelDto

- `username`: Required, string, 3-50 characters.
- `email`: Required, string, valid email format.
- `password`: Required, string, 6-100 characters.

### AuthResponseDto

- `userId`: UUID, unique identifier for the user.
- `token`: string, JWT token.
- `username`: string, username of the authenticated user.

### PersonDto

- `id`: integer, unique identifier for the person.
- `name`: string, 1-50 characters.
- `surname`: string, 1-50 characters.
- `age`: integer, 1-120.
- `email`: string, valid email format.
- `address`: string, 1-100 characters.
- `position`: object, contains position details.
- `salary`: object, contains salary details.
- `personDetails`: object, contains additional person details.

### CreateUpdatePersonDto

- `name`: Required, string, 1-50 characters.
- `surname`: Required, string, 1-50 characters.
- `age`: Required, integer, 1-120.
- `email`: Required, string, valid email format.
- `address`: Required, string, 1-100 characters.
- `personDetails`: Optional, object.
- `position`: Optional, object.
- `salary`: Optional, object.

### PersonDetailsDto

- `id`: integer, unique identifier for the person details.
- `personId`: integer, unique identifier for the person.
- `birthDay`: date, birthday of the person.
- `personCity`: string, 1-100 characters.

### CreateUpdatePersonDetailsDto

- `personId`: Required, integer, unique identifier for the person.
- `birthDay`: Required, date, birthday of the person.
- `personCity`: Required, string, 1-100 characters.

### DepartmentDto

- `departmentId`: integer, unique identifier for the department.
- `departmentName`: string, up to 50 characters.

### CreateUpdateDepartmentDto

- `departmentName`: Required, string, up to 50 characters.

### SalaryDto

- `salaryId`: integer, unique identifier for the salary.
- `amount`: integer, salary amount.

### CreateUpdateSalaryDto

- `amount`: Required, integer, greater than 0.

### SalaryWithFullNameDto

-   `personId`: integer, unique identifier for the person.
-   `fullName`: string, contains the full name of the person.
-   `amount`: decimal, contains the salary amount.

### PositionDto

-   `positionId`: integer, unique identifier for the position.
-   `name`: string, up to 50 characters.
-   `departmentId`: integer, unique identifier for the department.
-   `department`: object, contains department details.

### CreateUpdatePositionDto

-   `name`: Required, string, up to 50 characters.
-   `department`: object, contains department details.

## Error Handling

### User Already Exists

**Example Response:**

```json
{
  "error": "User already exists."
}
```

### Invalid Credentials

**Example Response:**

```json
{
  "error": "Invalid email or password."
}
```

### Person Not Found

**Example Response:**

```json
{
  "error": "Person with ID {id} was not found."
}
```

### Department Not Found

**Example Response:**

```json
{
  "error": "Department with ID {id} was not found."
}
```

### Salary Not Found

**Example Response:**

```json
{
  "error": "Salary for person with ID {personId} was not found."
}
```

## Additional Information

- **Services:** `AuthService`, `PersonService`, `DepartmentService`, and `SalaryService` handle the logic for user registration, login, managing persons, managing departments, and managing salaries.
- **Controllers:** `AuthController`, `PersonController`, `DepartmentController`, and `SalaryController` include endpoints for authentication, managing persons, managing departments, and managing salaries, respectively.