import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useForm, useFieldArray } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { getServers, ServerType, CreateBookDto, createBook } from "@/lib/books";
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
import { Loader2, Plus, Trash2, PencilIcon } from "lucide-react";
import { toast } from "sonner";
import { useUserStore } from "@/lib/userStore";

const chapterSchema = z.object({
  index: z.number(),
  title: z.string().min(1, "Title is required"),
  content: z.string().min(1, "Content is required"),
});

const bookSchema = z.object({
  server: z.number(),
  metadata: z.object({
    title: z.string().min(1, "Title is required"),
    description: z.string().min(1, "Description is required"),
    image_url: z.string().min(1, "Image URL is required"),
    chapters: z.array(chapterSchema),
  }),
});

type BookFormValues = z.infer<typeof bookSchema>;

export default function CreateBookForm() {
  const navigate = useNavigate();
  const user = useUserStore((state) => state.user);
  const [isLoading, setIsLoading] = useState(true);
  const [servers, setServers] = useState<ServerType[]>([]);

  const form = useForm<BookFormValues>({
    resolver: zodResolver(bookSchema),
    defaultValues: {
      server: 0,
      metadata: {
        title: "",
        image_url: "https://via.placeholder.com/400x600?text=No+Cover",
        description: "",
        chapters: [],
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
        const serverList = await getServers();
        setServers(serverList);
      } catch (error) {
        console.error("Error fetching data:", error);
        toast.error("Failed to load servers. Please try again.");
      } finally {
        setIsLoading(false);
      }
    };
    fetchData();
  }, [form]);

  const onSubmit = async (data: BookFormValues) => {
    if (!user) {
      toast.error("You must be logged in to create a book");
      return;
    }

    try {
      setIsLoading(true);

      const book: CreateBookDto = {
        ...data,
        author_id: user.id,
        audio_books: [],
      };

      await createBook(book);
      toast.success("Book created successfully");

      navigate(`/books`);
    } catch (error) {
      console.error("Error creating book:", error);
      toast.error("Failed to create book. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="container max-w-3xl py-8">
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl font-bold">Create Book</CardTitle>
          <CardDescription>Create a new book with chapters</CardDescription>
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
                      onValueChange={(value) => field.onChange(parseInt(value))}
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
                      <Loader2 className="h-4 w-4 animate-spin" />
                      Creating...
                    </>
                  ) : (
                    <>
                      <PencilIcon className="h-4 w-4" />
                      Create
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
