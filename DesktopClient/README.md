# DesktopClient

**AspHarmony DesktopClient** is an Electron application that provides a desktop interface for managing a digital library. This client allows users to access their libraries, manage books, and interact with other users from their desktop.

## Features

- **User Authentication**: Users can log in, register, and manage their accounts.
- **Library Management**: Create, join, and manage libraries.
- **Book Management**: Add, edit, and delete books within libraries.
- **Responsive Design**: Optimized for desktop environments.

## Tech Stack

- **Framework**: Electron
- **Frontend**: React with TypeScript
- **UI Components**: Shadcn-UI
- **State Management**: Zustand
- **API Communication**: Axios

## Getting Started

1. **Install Dependencies**:

   ```bash
   npm install
   ```

2. **Start the Development Server**:

   ```bash
   npm run vite:dev
   ```

3. **Run the Electron App**:
   ```bash
   npm run electron:dev
   ```

## API Endpoints

The DesktopClient interacts with the ASP.NET Core Web API for data management. Ensure the API is running to access the features of the DesktopClient.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue for any enhancements or bug fixes.

## License

This project is licensed under the MIT License.
