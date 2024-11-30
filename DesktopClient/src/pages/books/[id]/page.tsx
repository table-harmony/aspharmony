import { Book, getBookById } from "@/lib/books";

import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import { BookDetails } from "./_components/book-details";
import { BookDetailsSkeleton } from "./_components/book-details-skeleton";

export default function BookDetailsPage() {
  const { id } = useParams();
  const [book, setBook] = useState<Book | null>(null);

  useEffect(() => {
    if (!id) return;

    getBookById(parseInt(id)).then(setBook);
  }, [id]);

  return (
    <div className="min-h-screen py-8">
      <div className="mx-auto max-w-4xl px-4 sm:px-6">
        {book ? <BookDetails book={book} /> : <BookDetailsSkeleton />}
      </div>
    </div>
  );
}
