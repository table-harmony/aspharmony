import { Library, deleteLibrary, getLibrary } from "@/lib/libraries";
import { useEffect, useState } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import { Separator } from "@/components/ui/separator";
import { Button } from "@/components/ui/button";
import { ArrowLeft, Library as LibraryIcon, Trash2 } from "lucide-react";
import { scrollToTop } from "@/lib/utils";
import { Skeleton } from "@/components/ui/skeleton";
import { toast } from "sonner";

export default function DeleteLibraryPage() {
  const { id } = useParams();
  const [library, setLibrary] = useState<Library | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const navigate = useNavigate();

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

  const handleDelete = async () => {
    if (!id) {
      toast.error("Library ID not found");
      return;
    }

    try {
      setIsLoading(true);
      await deleteLibrary(parseInt(id));
      toast.success("Library deleted successfully");
      navigate("/libraries");
    } catch (error) {
      console.error("Error deleting library:", error);
      toast.error("Failed to delete library");
    } finally {
      setIsLoading(false);
    }
  };

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
              <Skeleton className="h-[200px] w-full" />
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
            <div className="w-24 h-24 rounded-full bg-destructive/10 flex items-center justify-center">
              <LibraryIcon className="h-12 w-12 text-destructive" />
            </div>
            <div className="space-y-4">
              <div className="space-y-2">
                <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl">
                  Delete "{library?.name || "Library"}"
                </h1>
                <p className="text-lg text-muted-foreground">
                  This action cannot be undone
                </p>
              </div>
              <div className="flex gap-4 justify-center">
                <Button size="lg" variant="outline" asChild>
                  <Link to={`/libraries/${id}/manage`} onClick={scrollToTop}>
                    <ArrowLeft className="mr-2 h-4 w-4" />
                    Back to Manage
                  </Link>
                </Button>
                <Button size="lg" variant="destructive" onClick={handleDelete}>
                  <Trash2 className="mr-2 h-4 w-4" />
                  Delete
                </Button>
              </div>
            </div>
          </section>

          <Separator />

          <section className="space-y-6">
            <div className="text-center text-muted-foreground">
              <p>This library will be permanently deleted.</p>
            </div>
          </section>
        </div>
      </div>
    </div>
  );
}
