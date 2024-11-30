import { type Book, getBooks } from "@/lib/books";

import { BookCard } from "./book-card";
import { BooksListSkeleton } from "./book-skeleton";

import { Link } from "react-router-dom";
import { useEffect, useState } from "react";

import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";

import emptyState from "@/assets/empty.svg";
import { scrollToTop } from "@/lib/utils";

export function BooksList() {
  const [books, setBooks] = useState<Book[] | null>(null);

  useEffect(() => {
    getBooks().then(setBooks);
  }, []);

  if (!books) {
    return <BooksListSkeleton />;
  }

  if (books.length === 0) {
    return (
      <div className="flex min-h-[400px] flex-col items-center justify-center gap-12 rounded-lg border border-dashed p-8 text-center animate-in fade-in-50">
        <div className="mx-auto flex max-w-[420px] flex-col items-center justify-center gap-2 text-center">
          <h2 className="text-xl font-semibold">
            There are no books available
          </h2>
          <p className="text-muted-foreground">
            Start by creating your first book
          </p>
        </div>
        <img src={emptyState} width={250} height={250} alt="No books" />
        <Button asChild>
          <Link to="/books/create" onClick={scrollToTop}>
            <Plus className="mr-2 h-4 w-4" />
            Create new book
          </Link>
        </Button>
      </div>
    );
  }

  return (
    <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
      {books.map((book) => (
        <BookCard key={book.id} book={book} />
      ))}
    </div>
  );
}
