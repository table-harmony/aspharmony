import { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { Book, getBookById, deleteBook } from "@/lib/books";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { AlertCircle, ArrowLeft, Loader2, Trash2 } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { toast } from "sonner";
import { useUserStore } from "@/lib/userStore";
import { scrollToTop } from "@/lib/utils";

export function DeleteBookForm({ id }: { id: number }) {
  const navigate = useNavigate();
  const [book, setBook] = useState<Book | null>(null);
  const user = useUserStore((state) => state.user);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (!id) return;

    getBookById(id)
      .then(setBook)
      .catch(() => toast.error("Failed to load book details"));
  }, [id]);

  const handleDelete = async () => {
    if (!id) return;

    if (!user || user.id !== book?.author.id) {
      toast.error("You are not the author of this book");
      return;
    }

    try {
      setIsLoading(true);
      await deleteBook(id);

      scrollToTop();
      navigate("/books", { replace: true });
    } catch {
      toast.error("Failed to delete the book");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-2xl font-bold text-center text-destructive">
          Delete "{book?.metadata.title}"
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <Alert variant="destructive">
          <AlertCircle className="h-4 w-4" />
          <AlertTitle>Warning</AlertTitle>
          <AlertDescription>
            Are you sure you want to delete this book? This action cannot be
            undone.
          </AlertDescription>
        </Alert>
      </CardContent>
      <CardFooter className="flex justify-between">
        <Button variant="outline" asChild>
          <Link to={`/books/${id}`} onClick={scrollToTop}>
            <ArrowLeft className="h-4 w-4" />
            Cancel
          </Link>
        </Button>
        <Button
          variant="destructive"
          onClick={handleDelete}
          disabled={isLoading}
        >
          {isLoading ? (
            <Loader2 className="h-4 w-4 animate-spin" />
          ) : (
            <Trash2 className="h-4 w-4" />
          )}
          {isLoading ? "Deleting..." : "Delete"}
        </Button>
      </CardFooter>
    </Card>
  );
}
