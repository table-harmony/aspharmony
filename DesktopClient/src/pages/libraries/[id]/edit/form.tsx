import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import {
  Library,
  UpdateLibraryDto,
  getLibrary,
  updateLibrary,
} from "@/lib/libraries";
import { useUserStore } from "@/lib/userStore";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Switch } from "@/components/ui/switch";
import { ArrowLeft, Loader2, Save } from "lucide-react";
import { toast } from "sonner";
import { scrollToTop } from "@/lib/utils";
import { Link } from "react-router-dom";

const librarySchema = z.object({
  name: z.string().min(1, "Name is required"),
  allow_copies: z.boolean(),
});

type LibraryFormValues = z.infer<typeof librarySchema>;

export function EditLibraryForm({ id }: { id: number }) {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [library, setLibrary] = useState<Library | null>(null);
  const user = useUserStore((state) => state.user);

  const form = useForm<LibraryFormValues>({
    resolver: zodResolver(librarySchema),
    defaultValues: {
      name: "",
      allow_copies: false,
    },
  });

  useEffect(() => {
    if (!id) return;

    const fetchLibrary = async () => {
      try {
        const data = await getLibrary(id);
        setLibrary(data);

        form.reset({
          name: data.name,
          allow_copies: data.allow_copies,
        });
      } catch {
        toast.error("Failed to load library details");
        navigate("/libraries");
      }
    };

    fetchLibrary();
  }, [id, form, navigate]);

  const onSubmit = async (data: LibraryFormValues) => {
    if (!id || !library) return;

    const userMembership = library.members.find((m) => m.user.id === user?.id);
    if (!userMembership || userMembership.role !== "Manager") {
      toast.error("You must be a manager to edit this library");
      return;
    }

    try {
      setIsLoading(true);

      const updateDto: UpdateLibraryDto = {
        id,
        name: data.name,
        allow_copies: data.allow_copies,
      };

      await updateLibrary(updateDto);
      toast.success("Library updated successfully");

      scrollToTop();
      navigate(`/libraries/${id}`);
    } catch (error) {
      console.error("Error updating library:", error);
      toast.error("Failed to update library. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-2xl font-bold">Edit Library</CardTitle>
        <CardDescription>Update library settings</CardDescription>
      </CardHeader>
      <CardContent>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
            <FormField
              control={form.control}
              name="name"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Name</FormLabel>
                  <FormControl>
                    <Input placeholder="Enter library name" {...field} />
                  </FormControl>
                  <FormDescription>
                    Choose a name for your library
                  </FormDescription>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="allow_copies"
              render={({ field }) => (
                <FormItem className="flex flex-row items-center justify-between rounded-lg border p-4">
                  <div className="space-y-0.5">
                    <FormLabel className="text-base">Allow Copies</FormLabel>
                    <FormDescription>
                      Allow members to make copies of books in this library
                    </FormDescription>
                  </div>
                  <FormControl>
                    <Switch
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                  </FormControl>
                </FormItem>
              )}
            />

            <CardFooter className="flex justify-between px-0">
              <Button variant="outline" asChild>
                <Link to={`/libraries/${id}`} onClick={scrollToTop}>
                  <ArrowLeft className="h-4 w-4 mr-2" />
                  Cancel
                </Link>
              </Button>
              <Button type="submit" disabled={isLoading}>
                {isLoading ? (
                  <>
                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                    Saving...
                  </>
                ) : (
                  <>
                    <Save className="mr-2 h-4 w-4" />
                    Save Changes
                  </>
                )}
              </Button>
            </CardFooter>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
}
