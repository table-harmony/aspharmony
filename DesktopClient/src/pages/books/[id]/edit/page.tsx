import { Book, getBookById } from "@/lib/books";
import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { Separator } from "@/components/ui/separator";
import EditBookForm from "./form";
import { Button } from "@/components/ui/button";
import { ArrowLeft } from "lucide-react";
import { scrollToTop } from "@/lib/utils";
import { Skeleton } from "@/components/ui/skeleton";

export default function EditBookPage() {
  const { id } = useParams();
  const [book, setBook] = useState<Book | null>(null);
  const [isLoading, setIsLoading] = useState(true);

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
              <Skeleton className="h-[600px] w-full" />
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
                  Edit "{book?.metadata.title || "Book"}"
                </h1>
                <p className="text-lg text-muted-foreground">
                  Update your book's details and content
                </p>
              </div>
              <Button variant="outline" asChild>
                <Link to={`/books/${id}`} onClick={scrollToTop}>
                  <ArrowLeft className="mr-2 h-4 w-4" />
                  Back to Book
                </Link>
              </Button>
            </div>
          </section>

          <Separator />

          <section className="space-y-6">
            {book && <EditBookForm book={book} />}
          </section>
        </div>
      </div>
    </div>
  );
}
