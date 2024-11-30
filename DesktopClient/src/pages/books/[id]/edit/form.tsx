import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useForm, useFieldArray } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import {
  updateBook,
  getBookById,
  getServers,
  ServerType,
  UpdateBookDto,
} from "@/lib/books";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
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
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Loader2, Save, Plus, Trash2 } from "lucide-react";
import { toast } from "sonner";
import { useUserStore } from "@/lib/userStore";
import { scrollToTop } from "@/lib/utils";

const chapterSchema = z.object({
  index: z.number(),
  title: z.string().min(1, "Title is required"),
  content: z.string().min(1, "Content is required"),
});

const bookSchema = z.object({
  id: z.number(),
  author: z.object({
    id: z.string(),
    username: z.string(),
    email: z.string(),
    phone_number: z.string(),
  }),
  server: z.number(),
  metadata: z.object({
    id: z.number(),
    title: z.string().min(1, "Title is required"),
    description: z.string().min(1, "Description is required"),
    chapters: z.array(chapterSchema),
    image_url: z.string().min(1, "Image is required"),
  }),
});

type BookFormValues = z.infer<typeof bookSchema>;

export default function EditBookForm({ id }: { id: number }) {
  const user = useUserStore((state) => state.user);
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);
  const [servers, setServers] = useState<ServerType[]>([]);

  const form = useForm<BookFormValues>({
    resolver: zodResolver(bookSchema),
    defaultValues: {
      id: 0,
      server: 0,
      metadata: {
        id: 0,
        title: "",
        description: "",
        chapters: [],
        image_url: "",
      },
    },
  });

  const { fields, append, remove } = useFieldArray({
    control: form.control,
    name: "metadata.chapters",
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [book, serverList] = await Promise.all([
          getBookById(id),
          getServers(),
        ]);
        if (book) {
          form.reset(book);
        }
        setServers(serverList);
      } catch (error) {
        console.error("Error fetching data:", error);
        toast.error("Failed to load book data. Please try again.");
      } finally {
        setIsLoading(false);
      }
    };
    fetchData();
  }, [id, form]);

  const onSubmit = async (data: BookFormValues) => {
    if (!user || user.id !== data.author.id) {
      toast.error("You are not the author of this book");
      return;
    }

    try {
      setIsLoading(true);

      const updateData: UpdateBookDto = {
        id: data.id,
        server: data.server,
        author_id: data.author.id,
        metadata: data.metadata,
      };
      await updateBook(updateData);

      toast.success("Book updated successfully");

      scrollToTop();
      navigate(`/books/${id}`);
    } catch (error) {
      console.error("Error updating book:", error);
      toast.error("Failed to update book. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="container max-w-3xl py-8">
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl font-bold">Edit Book</CardTitle>
          <CardDescription>
            Update your book's details and chapters
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
              <FormField
                control={form.control}
                name="metadata.title"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Title</FormLabel>
                    <FormControl>
                      <Input placeholder="Enter book title" {...field} />
                    </FormControl>
                    <FormDescription>The title of your book</FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="metadata.description"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Description</FormLabel>
                    <FormControl>
                      <Textarea
                        placeholder="Enter book description"
                        className="min-h-[100px]"
                        {...field}
                      />
                    </FormControl>
                    <FormDescription>
                      A brief explanation of your book
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="server"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Server</FormLabel>
                    <Select
                      onValueChange={field.onChange}
                      defaultValue={field.value.toString()}
                    >
                      <FormControl>
                        <SelectTrigger>
                          <SelectValue placeholder="Select a server" />
                        </SelectTrigger>
                      </FormControl>
                      <SelectContent>
                        {servers.map((server) => (
                          <SelectItem
                            key={server.id}
                            value={server.id.toString()}
                          >
                            {server.display_name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                    <FormDescription>
                      Choose the server for this book
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <div>
                <h3 className="text-lg font-medium mb-2">Chapters</h3>
                <Button
                  type="button"
                  variant="outline"
                  size="sm"
                  className="mb-4"
                  onClick={() =>
                    append({ index: fields.length, title: "", content: "" })
                  }
                >
                  <Plus className="h-4 w-4" />
                  Add
                </Button>
                <div className="grid gap-6 sm:grid-cols-1 xl:grid-cols-2">
                  {fields.map((field, index) => (
                    <div
                      key={field.id}
                      className="flex flex-col gap-4 mb-4 p-4 border rounded-md"
                    >
                      <h3 className="text-lg font-medium">
                        Chapter {index + 1}
                      </h3>
                      <FormField
                        control={form.control}
                        name={`metadata.chapters.${index}.title`}
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Title</FormLabel>
                            <FormControl>
                              <Input
                                placeholder="Enter chapter title"
                                {...field}
                              />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                      <FormField
                        control={form.control}
                        name={`metadata.chapters.${index}.content`}
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel>Content</FormLabel>
                            <FormControl>
                              <Textarea
                                placeholder="Enter chapter content"
                                className="min-h-[100px]"
                                {...field}
                              />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                      <Button
                        type="button"
                        variant="destructive"
                        size="sm"
                        onClick={() => remove(index)}
                      >
                        <Trash2 className="h-4 w-4 mr-2" />
                        Remove
                      </Button>
                    </div>
                  ))}
                </div>
              </div>
              <CardFooter className="flex justify-end px-0">
                <Button type="submit" disabled={isLoading}>
                  {isLoading ? (
                    <>
                      <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                      Updating...
                    </>
                  ) : (
                    <>
                      <Save className="mr-2 h-4 w-4" />
                      Update
                    </>
                  )}
                </Button>
              </CardFooter>
            </form>
          </Form>
        </CardContent>
      </Card>
    </div>
  );
}
