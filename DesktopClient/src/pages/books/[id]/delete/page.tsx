import { Book, getBookById, deleteBook } from "@/lib/books";
import { useEffect, useState } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import { Separator } from "@/components/ui/separator";
import { Button } from "@/components/ui/button";
import { ArrowLeft, Loader2Icon, Trash2Icon } from "lucide-react";
import { scrollToTop } from "@/lib/utils";
import { Skeleton } from "@/components/ui/skeleton";
import { toast } from "sonner";

export default function DeleteBookPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [book, setBook] = useState<Book | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isDeleting, setIsDeleting] = useState(false);

  useEffect(() => {
    if (!id) return;

    const fetchBook = async () => {
      try {
        const data = await getBookById(parseInt(id));
        setBook(data);
      } catch (error) {
        console.error("Error fetching book:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchBook();
  }, [id]);

  const handleDelete = async () => {
    if (!id) {
      toast.error("Book ID not found");
      return;
    }

    try {
      setIsDeleting(true);
      await deleteBook(parseInt(id));
      toast.success("Book deleted successfully");
      navigate("/books");
    } catch (error) {
      console.error("Error deleting book:", error);
      toast.error("Failed to delete book");
    } finally {
      setIsDeleting(false);
    }
  };

  if (isLoading) {
    return (
      <div className="py-8">
        <div className="container max-w-4xl mx-auto px-4 sm:px-6">
          <div className="flex flex-col gap-8">
            <section className="flex flex-col items-center text-center gap-8">
              <div className="space-y-8">
                <Skeleton className="w-48 h-48 rounded-lg mx-auto" />
              </div>
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
            <div className="space-y-8">
              {book?.metadata.image_url && (
                <img
                  src={book.metadata.image_url}
                  alt={book.metadata.title}
                  className="w-48 h-48 object-cover rounded-lg mx-auto"
                />
              )}
            </div>
            <div className="space-y-4">
              <div className="space-y-2">
                <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl">
                  Delete "{book?.metadata.title || "Book"}"
                </h1>
                <p className="text-lg text-muted-foreground">
                  This action cannot be undone
                </p>
              </div>
              <div className="flex gap-4 justify-center">
                <Button size="lg" variant="outline" asChild>
                  <Link to={`/books/${id}`} onClick={scrollToTop}>
                    <ArrowLeft className="mr-2 h-4 w-4" />
                    Back to Book
                  </Link>
                </Button>
                <Button
                  size="lg"
                  variant="destructive"
                  onClick={handleDelete}
                  disabled={isDeleting}
                >
                  {isDeleting ? (
                    <>
                      <Loader2Icon className="mr-2 h-4 w-4 animate-spin" />
                      Deleting...
                    </>
                  ) : (
                    <>
                      <Trash2Icon className="mr-2 h-4 w-4" />
                      Delete
                    </>
                  )}
                </Button>
              </div>
            </div>
          </section>

          <Separator />

          <section className="space-y-6">
            <div className="text-center text-muted-foreground">
              <p>
                All book content and associated data will be permanently
                deleted.
              </p>
              <p>
                This includes chapters, audio files, and any other related
                content.
              </p>
            </div>
          </section>
        </div>
      </div>
    </div>
  );
}
