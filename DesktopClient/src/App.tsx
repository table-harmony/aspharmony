import { HashRouter as Router, Routes, Route } from "react-router-dom";
import { Toaster } from "sonner";

import { SiteLayout } from "@/components/site-layout";

import BooksPage from "@/pages/books/page";
import BookDetailsPage from "@/pages/books/[id]/page";
import DeleteBookPage from "@/pages/books/[id]/delete/page";
import EditBookPage from "@/pages/books/[id]/edit/page";
import CreateBookPage from "@/pages/books/create/page";

import LoginPage from "@/pages/auth/login/page";
import RegisterPage from "@/pages/auth/register/page";

import HomePage from "@/pages/page";
import LibrariesPage from "@/pages/libraries/page";
import LibraryDetailsPage from "@/pages/libraries/[id]/page";
import CreateLibraryPage from "@/pages/libraries/create/page";
import ManageLibraryPage from "@/pages/libraries/[id]/manage/page";
import DeleteLibraryPage from "./pages/libraries/[id]/delete/page";
import EditLibraryPage from "./pages/libraries/[id]/edit/page";

export default function App() {
  return (
    <Router>
      <SiteLayout>
        <Routes>
          <Route path="/" element={<HomePage />} />

          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />

          <Route path="/books" element={<BooksPage />} />
          <Route path="/books/create" element={<CreateBookPage />} />
          <Route path="/books/:id" element={<BookDetailsPage />} />
          <Route path="/books/:id/edit" element={<EditBookPage />} />
          <Route path="/books/:id/delete" element={<DeleteBookPage />} />

          <Route path="/libraries" element={<LibrariesPage />} />
          <Route path="/libraries/create" element={<CreateLibraryPage />} />
          <Route path="/libraries/:id" element={<LibraryDetailsPage />} />
          <Route path="/libraries/:id/edit" element={<EditLibraryPage />} />
          <Route path="/libraries/:id/delete" element={<DeleteLibraryPage />} />
          <Route path="/libraries/:id/manage" element={<ManageLibraryPage />} />
        </Routes>
      </SiteLayout>
      <Toaster />
    </Router>
  );
}
