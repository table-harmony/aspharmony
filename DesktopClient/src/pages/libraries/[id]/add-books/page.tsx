import { useParams } from "react-router-dom";
import { AddBooksToLibrary } from "./add-books";

export default function AddBooksToLibraryPage() {
  const { id } = useParams();

  if (!id) return null;

  return (
    <div className="min-h-screen py-8">
      <div className="container max-w-6xl mx-auto px-4 sm:px-6">
        <AddBooksToLibrary id={parseInt(id)} />
      </div>
    </div>
  );
}
