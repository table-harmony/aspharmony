import { useState, useEffect } from "react";
import { useNavigate, Link } from "react-router-dom";
import { Library, getLibrary, deleteLibrary } from "@/lib/libraries";
import { useUserStore } from "@/lib/userStore";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { AlertCircle, ArrowLeft, Loader2, Trash2 } from "lucide-react";
import { toast } from "sonner";
import { scrollToTop } from "@/lib/utils";

export function DeleteLibraryForm({ id }: { id: number }) {
  const navigate = useNavigate();
  const [library, setLibrary] = useState<Library | null>(null);
  const user = useUserStore((state) => state.user);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (!id) return;

    getLibrary(id)
      .then(setLibrary)
      .catch(() => toast.error("Failed to load library details"));
  }, [id]);

  const handleDelete = async () => {
    if (!id) return;

    const userMembership = library?.members.find((m) => m.user.id === user?.id);
    if (!userMembership || userMembership.role !== "Manager") {
      toast.error("You must be a manager to delete this library");
      return;
    }

    try {
      setIsLoading(true);
      await deleteLibrary(id);

      scrollToTop();
      navigate("/libraries", { replace: true });
      toast.success("Library deleted successfully");
    } catch {
      toast.error("Failed to delete the library");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-2xl font-bold text-center text-destructive">
          Delete "{library?.name}"
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <Alert variant="destructive">
          <AlertCircle className="h-4 w-4" />
          <AlertTitle>Warning</AlertTitle>
          <AlertDescription>
            Are you sure you want to delete this library? This action cannot be
            undone and will remove access for all members.
          </AlertDescription>
        </Alert>
      </CardContent>
      <CardFooter className="flex justify-between">
        <Button variant="outline" asChild>
          <Link to={`/libraries/${id}`} onClick={scrollToTop}>
            <ArrowLeft className="h-4 w-4 mr-2" />
            Cancel
          </Link>
        </Button>
        <Button
          variant="destructive"
          onClick={handleDelete}
          disabled={isLoading}
        >
          {isLoading ? (
            <Loader2 className="h-4 w-4 mr-2 animate-spin" />
          ) : (
            <Trash2 className="h-4 w-4 mr-2" />
          )}
          {isLoading ? "Deleting..." : "Delete"}
        </Button>
      </CardFooter>
    </Card>
  );
}
