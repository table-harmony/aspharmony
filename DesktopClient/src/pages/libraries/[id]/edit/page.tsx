import { getLibrary, Library } from "@/lib/libraries";
import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { EditLibraryForm } from "./form";
import { Separator } from "@/components/ui/separator";
import { Button } from "@/components/ui/button";
import { ArrowLeft, Library as LibraryIcon } from "lucide-react";
import { scrollToTop } from "@/lib/utils";
import { Skeleton } from "@/components/ui/skeleton";

export default function EditLibraryPage() {
  const { id } = useParams();
  const [library, setLibrary] = useState<Library | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (!id) return;

    const fetchLibrary = async () => {
      try {
        const data = await getLibrary(parseInt(id));
        setLibrary(data);
      } catch (error) {
        console.error("Error fetching library:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchLibrary();
  }, [id]);

  if (isLoading) {
    return (
      <div className="py-8">
        <div className="container max-w-4xl mx-auto px-4 sm:px-6">
          <div className="flex flex-col gap-8">
            <section className="flex flex-col items-center text-center gap-8">
              <Skeleton className="w-24 h-24 rounded-full" />
              <div className="space-y-4">
                <div className="space-y-2">
                  <Skeleton className="h-8 w-64 mx-auto" />
                  <Skeleton className="h-4 w-48 mx-auto" />
                </div>
                <Skeleton className="h-10 w-32 mx-auto" />
              </div>
            </section>
            <Separator />
            <section className="space-y-6">
              <Skeleton className="h-[400px] w-full" />
            </section>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="py-8">
      <div className="container max-w-4xl mx-auto px-4 sm:px-6">
        <div className="flex flex-col gap-8">
          <section className="flex flex-col items-center text-center gap-8">
            <div className="w-24 h-24 rounded-full bg-primary/10 flex items-center justify-center">
              <LibraryIcon className="h-12 w-12 text-primary" />
            </div>
            <div className="space-y-4">
              <div className="space-y-2">
                <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl">
                  Edit "{library?.name || "Library"}"
                </h1>
                <p className="text-lg text-muted-foreground">
                  Update your library's details and settings
                </p>
              </div>
              <Button variant="outline" asChild>
                <Link to={`/libraries/${id}/manage`} onClick={scrollToTop}>
                  <ArrowLeft className="mr-2 h-4 w-4" />
                  Back to Manage
                </Link>
              </Button>
            </div>
          </section>

          <Separator />

          <section className="space-y-6">
            {id && <EditLibraryForm id={parseInt(id)} />}
          </section>
        </div>
      </div>
    </div>
  );
}
