import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { CreateLibraryDto, createLibrary, joinLibrary } from "@/lib/libraries";
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
import { Loader2, Library } from "lucide-react";
import { toast } from "sonner";
import { scrollToTop } from "@/lib/utils";
import { useUserStore } from "@/lib/userStore";

const librarySchema = z.object({
  name: z.string().min(1, "Name is required"),
  allow_copies: z.boolean().default(false),
});

type LibraryFormValues = z.infer<typeof librarySchema>;

export function CreateLibraryForm() {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const user = useUserStore((state) => state.user);

  const form = useForm<LibraryFormValues>({
    resolver: zodResolver(librarySchema),
    defaultValues: {
      name: "",
      allow_copies: false,
    },
  });

  const onSubmit = async (data: LibraryFormValues) => {
    if (!user) {
      toast.error("You must be logged in to create a library");
      return;
    }

    try {
      setIsLoading(true);

      const library: CreateLibraryDto = {
        name: data.name,
        allow_copies: data.allow_copies,
      };

      const createdLibrary = await createLibrary(library);
      await joinLibrary(createdLibrary.id, user.id, "Manager");

      toast.success("Library created successfully");

      scrollToTop();
      navigate("/libraries");
    } catch (error) {
      console.error("Error creating library:", error);
      toast.error("Failed to create library. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-2xl font-bold">Create Library</CardTitle>
        <CardDescription>Create a new library to share books</CardDescription>
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

            <CardFooter className="flex justify-end px-0">
              <Button type="submit" disabled={isLoading}>
                {isLoading ? (
                  <>
                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                    Creating...
                  </>
                ) : (
                  <>
                    <Library className="mr-2 h-4 w-4" />
                    Create Library
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
