import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { Toaster } from "sonner";

import { SiteLayout } from "@/components/site-layout";

import BooksPage from "@/pages/books/page";
import BookDetailsPage from "@/pages/books/[id]/page";
import DeleteBookPage from "@/pages/books/[id]/delete/page";
import EditBookPage from "@/pages/books/[id]/edit/page";
import CreateBookPage from "@/pages/books/create/page";

import LoginPage from "@/pages/auth/login";
import RegisterPage from "@/pages/auth/register";

import HomePage from "@/pages/page";

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
        </Routes>
      </SiteLayout>
      <Toaster />
    </Router>
  );
}
