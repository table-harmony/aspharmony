import { Book, getBookById } from "@/lib/books";
import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { BookDetails } from "./_components/book-details";
import { Separator } from "@/components/ui/separator";
import { BookOpen, PencilIcon, Trash2 } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";
import { Button } from "@/components/ui/button";
import { useUserStore } from "@/lib/userStore";

export default function BookPage() {
  const { id } = useParams();
  const [book, setBook] = useState<Book | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const user = useUserStore((state) => state.user);

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
              <Skeleton className="w-24 h-24 rounded-full" />
              <div className="space-y-4">
                <div className="space-y-2">
                  <Skeleton className="h-8 w-64 mx-auto" />
                  <Skeleton className="h-4 w-48 mx-auto" />
                  <Skeleton className="h-4 w-32 mx-auto" />
                </div>
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
              <BookOpen className="h-12 w-12 text-primary" />
            </div>
            <div className="space-y-4">
              <div className="space-y-2">
                <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl">
                  {book?.metadata.title || "Book Details"}
                </h1>
                <p className="text-lg text-muted-foreground">
                  By {book?.author.username || "Unknown Author"}
                </p>
              </div>
            </div>
            {user?.id === book?.author.id && (
              <div className="flex gap-4 justify-center">
                <Button size="lg" variant="outline" asChild>
                  <Link to={`/books/${id}/edit`}>
                    <PencilIcon className="mr-2 h-4 w-4" />
                    Edit
                  </Link>
                </Button>
                <Button size="lg" variant="destructive" asChild>
                  <Link to={`/books/${id}/delete`}>
                    <Trash2 className="mr-2 h-4 w-4" />
                    Delete
                  </Link>
                </Button>
              </div>
            )}
          </section>

          <Separator />

          <section className="space-y-6">
            {book && <BookDetails book={book} />}
          </section>
        </div>
      </div>
    </div>
  );
}
