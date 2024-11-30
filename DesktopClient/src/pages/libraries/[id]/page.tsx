import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Library, getLibrary } from "@/lib/libraries";
import { LibraryDetails } from "./_components/library-details";
import { LibraryDetailsSkeleton } from "./_components/library-details-skeleton";

export default function LibraryDetailsPage() {
  const { id } = useParams();
  const [library, setLibrary] = useState<Library | null>(null);

  useEffect(() => {
    if (!id) return;
    getLibrary(parseInt(id)).then(setLibrary);
  }, [id]);

  return (
    <div className="min-h-screen py-8">
      <div className="mx-auto max-w-4xl px-4 sm:px-6">
        {library ? (
          <LibraryDetails library={library} />
        ) : (
          <LibraryDetailsSkeleton />
        )}
      </div>
    </div>
  );
}
