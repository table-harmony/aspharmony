import { useState, useEffect } from "react";
import { Library, getLibraries } from "@/lib/libraries";
import { LibraryCard } from "./library-card";

import { Link } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import emptyState from "@/assets/empty.svg";
import { scrollToTop } from "@/lib/utils";
import { LibrariesListSkeleton } from "./libraries-skeleton";

export function LibrariesList() {
  const [libraries, setLibraries] = useState<Library[] | null>(null);

  useEffect(() => {
    getLibraries().then(setLibraries);
  }, []);

  if (!libraries) {
    return <LibrariesListSkeleton />;
  }

  if (libraries.length === 0) {
    return (
      <div className="flex min-h-[400px] flex-col items-center justify-center gap-12 rounded-lg border border-dashed p-8 text-center animate-in fade-in-50">
        <div className="mx-auto flex max-w-[420px] flex-col items-center justify-center gap-2 text-center">
          <h2 className="text-xl font-semibold">No libraries available</h2>
          <p className="text-muted-foreground">
            Create your first library to start sharing books
          </p>
        </div>
        <img src={emptyState} width={250} height={250} alt="No libraries" />
        <Button asChild>
          <Link to="/libraries/create" onClick={scrollToTop}>
            <Plus className="h-4 w-4" />
            Create new library
          </Link>
        </Button>
      </div>
    );
  }

  return (
    <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
      {libraries.map((library) => (
        <LibraryCard key={library.id} library={library} />
      ))}
    </div>
  );
}
