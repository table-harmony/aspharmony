import { useParams } from "react-router-dom";

import { DeleteBookForm } from "./form";

export default function DeleteBookPage() {
  const { id } = useParams();

  if (!id) return null;

  return (
    <div className="min-h-screen py-8">
      <div className="mx-auto max-w-4xl px-4 sm:px-6">
        <DeleteBookForm id={parseInt(id)} />
      </div>
    </div>
  );
}
