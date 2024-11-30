import { useParams } from "react-router-dom";
import { EditLibraryForm } from "./form";

export default function EditLibraryPage() {
  const { id } = useParams();

  if (!id) return null;

  return (
    <div className="min-h-screen py-8">
      <div className="container max-w-2xl mx-auto px-4 sm:px-6">
        <EditLibraryForm id={parseInt(id)} />
      </div>
    </div>
  );
}
