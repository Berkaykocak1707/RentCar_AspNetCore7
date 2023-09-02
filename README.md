# RentCar_AspNetCore7 Project

## Overview

This project is an ASP.NET Core version of a car rental service. Users can browse through available cars and rent them for a specified period. The admin panel allows for the approval of reservations, which updates the corresponding car's information.

## Features

- Browse available cars with filtering options
- Rent a car for a specified duration
- Admin interface: Admins can approve reservations and update car information

## Installation & Setup

### Prerequisites

- .NET Core SDK
- Visual Studio or Visual Studio Code

### Steps

1. **Clone the Repository**

    ```bash
    git clone https://github.com/your-username/RentCar_AspNetCore7.git
    ```

2. **Navigate to the Project Directory**

    ```bash
    cd RentCar_AspNetCore7
    ```

3. **Restore Dependencies**

    ```bash
    dotnet restore
    ```

4. **Run the Application**

    ```bash
    dotnet run
    ```

    You can now access the application at `http://localhost:5000/` or `https://localhost:5001/`.

## Usage

- To browse available cars, go to the homepage.
- To rent a car, you can make a reservation.
- Reservations are activated once approved from the admin panel.

## Database

The project uses a local database for development. You can configure a different database by modifying the `appsettings.json` file.

