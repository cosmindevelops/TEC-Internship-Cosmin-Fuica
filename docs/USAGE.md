# API Usage Guide

## Table of Contents

1. [Introduction](#introduction)
2. [Base URLs](#base-urls)
3. [Authentication](#authentication)
   - [Register](#register)
   - [Login](#login)
4. [Dashboard](#dashboard)
5. [Person Management](#person-management)
   - [View All Persons](#view-all-persons)
   - [Edit Person](#edit-person)
   - [Delete Person](#delete-person)
   - [Create Person](#create-person)
6. [Department Management](#department-management)
   - [View All Departments](#view-all-departments)
   - [Edit Department](#edit-department)
   - [Delete Department](#delete-department)
   - [Create Department](#create-department)
7. [Salary Management](#salary-management)
   - [View All Salaries](#view-all-salaries)
   - [Edit Salary](#edit-salary)
8. [Person & Department Management](#person--department-management)

## Introduction

This guide provides detailed instructions on how to use the application for managing authentication, persons, departments, and salaries.

## Base URLs

- **API Application:**
  - Development: `http://localhost:6070`
- **Web Application:**
  - Development: `https://localhost:44315`

## Authentication

### Register

1. Start the application.
2. Go to `https://localhost:44315`.
3. Click on the "Register" link inside the login window.
4. Fill in the username, email, and password fields.
5. Click the "Register" button.
6. If the registration is successful, the user will be logged in automatically and redirected to the Dashboard page.

### Login

1. Start the application.
2. Go to `https://localhost:44315`.
3. Enter your email and password.
4. Click the "Login" button.
5. Upon successful login, you will be redirected to the Dashboard page.

## Dashboard

- On the first page of the application, you will see the Dashboard, which displays the total number of Persons and Departments from the database.
- The Dashboard also includes 4 quick action buttons that redirect you to specific pages when clicked.

## Person Management

### View All Persons

1. Click on "Person" in the navbar.
2. You will be redirected to `/Person`.
3. A table displaying all users and their details from the database will be shown.
4. You can filter the columns displayed in the table by clicking the "Display" button in the top right corner.

### Edit Person

1. In the table under the "Action" column, click the edit button (pencil icon) next to the person you want to edit.
2. The row will transform into input fields where you can modify the details.
3. After making changes, click the save icon to save the changes or the X button to cancel.

### Delete Person

1. In the table under the "Action" column, click the delete button (trash icon) next to the person you want to delete.
2. The person will be deleted from the database.

### Create Person

1. Navigate to the Person & Department Management section (explained later).

## Department Management

### View All Departments

1. Click on "Department" in the navbar.
2. You will be redirected to `/Department`.
3. A table displaying all departments from the database will be shown.

### Edit Department

1. In the table under the "Action" column, click the edit button (pencil icon) next to the department you want to edit.
2. The department name will become an input field where you can change the name.
3. After making changes, click the save icon to save the changes or the X button to cancel.

### Delete Department

1. In the table under the "Action" column, click the delete button (trash icon) next to the department you want to delete.
2. The department will be deleted from the database.

### Create Department

1. At the bottom of the department table, there is an input row labeled "New department name".
2. Type in the new department name.
3. Click the "+" icon to add the department to the database.
4. Note: You cannot create a department that already exists in the database.

## Salary Management

### View All Salaries

1. Click on "Salary" in the navbar.
2. You will be redirected to `/Salary`.
3. A table displaying all persons and their salaries from the database will be shown.

### Edit Salary

1. In the table under the "Action" column, click the edit button (pencil icon) next to the salary you want to edit.
2. The salary amount will become an input field where you can change the amount.
3. After making changes, click the save icon to save the changes or the X button to cancel.

## Person & Department Management

### Create Person

1. Click on "Person & Department Management" in the navbar.
2. You will be redirected to `/Person/Create`.
3. Fill in the form under "Create Person" with the required details.
4. Click the "Create Person" button to add the person to the database.

### Create Department

1. Click on "Person & Department Management" in the navbar.
2. You will be redirected to `/Person/Create`.
3. Fill in the form under "Create Department" with the required details.
4. Click the "Create Department" button to add the department to the database.