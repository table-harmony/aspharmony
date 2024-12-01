import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import {
  Library,
  promoteMember,
  demoteMember,
  removeMember,
} from "@/lib/libraries";
import { useUserStore } from "@/lib/userStore";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Shield, ShieldOff, UserMinus } from "lucide-react";
import { toast } from "sonner";
import { Pagination } from "@/components/ui/pagination";
import { Input } from "@/components/ui/input";

export function ManageMembers({ library }: { library: Library }) {
  const { id } = useParams();
  const user = useUserStore((state) => state.user);
  const [isLoading, setIsLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [search, setSearch] = useState("");
  const itemsPerPage = 3;

  const userMembership = library?.members.find((m) => m.user.id === user?.id);
  const isManager = userMembership?.role === "Manager";

  const handlePromote = async (userId: string) => {
    if (!id || !isManager) return;
    try {
      setIsLoading(true);

      await promoteMember(parseInt(id), userId);
      library.members = library.members.map((m) =>
        m.user.id === userId ? { ...m, role: "Manager" } : m
      );

      toast.success("Member promoted to manager");
    } catch {
      toast.error("Failed to promote member");
    } finally {
      setIsLoading(false);
    }
  };

  const handleDemote = async (userId: string) => {
    if (!id || !isManager) return;
    try {
      setIsLoading(true);

      await demoteMember(parseInt(id), userId);
      library.members = library.members.map((m) =>
        m.user.id === userId ? { ...m, role: "Member" } : m
      );

      toast.success("Manager demoted to member");
    } catch {
      toast.error("Failed to demote manager");
    } finally {
      setIsLoading(false);
    }
  };

  const handleRemove = async (userId: string) => {
    if (!id || !isManager) return;
    try {
      setIsLoading(true);

      await removeMember(parseInt(id), userId);
      library.members = library.members.filter((m) => m.user.id !== userId);

      toast.success("Member removed from library");
    } catch {
      toast.error("Failed to remove member");
    } finally {
      setIsLoading(false);
    }
  };

  const filteredMembers =
    library?.members.filter(
      (member) =>
        member.user.username.toLowerCase().includes(search.toLowerCase()) ||
        member.user.email.toLowerCase().includes(search.toLowerCase())
    ) || [];

  const totalPages = Math.ceil(filteredMembers.length / itemsPerPage);
  const paginatedMembers = filteredMembers.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  useEffect(() => {
    setCurrentPage(1);
  }, [search]);

  return (
    <div className="space-y-8">
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl font-bold">Members</CardTitle>
          <CardDescription>
            Manage members and their roles in "{library?.name}"
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <Input
            placeholder="Search members..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className="max-w-sm"
          />

          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Username</TableHead>
                <TableHead>Email</TableHead>
                <TableHead>Role</TableHead>
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {paginatedMembers.map((member) => (
                <TableRow key={member.id}>
                  <TableCell className="font-medium">
                    {member.user.username}
                  </TableCell>
                  <TableCell>{member.user.email}</TableCell>
                  <TableCell>{member.role}</TableCell>
                  <TableCell className="text-right">
                    {member.user.id !== user?.id && (
                      <div className="flex justify-end gap-2">
                        {member.role === "Member" ? (
                          <Button
                            variant="outline"
                            size="sm"
                            onClick={() => handlePromote(member.user.id)}
                            disabled={isLoading}
                          >
                            <Shield className="h-4 w-4" />
                            <span className="sr-only">Promote</span>
                          </Button>
                        ) : (
                          <Button
                            variant="outline"
                            size="sm"
                            onClick={() => handleDemote(member.user.id)}
                            disabled={isLoading}
                          >
                            <ShieldOff className="h-4 w-4" />
                            <span className="sr-only">Demote</span>
                          </Button>
                        )}
                        <Button
                          variant="outline"
                          size="sm"
                          onClick={() => handleRemove(member.user.id)}
                          disabled={isLoading}
                        >
                          <UserMinus className="h-4 w-4" />
                          <span className="sr-only">Remove</span>
                        </Button>
                      </div>
                    )}
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>

          <div className="flex items-center justify-between">
            <div className="text-sm text-muted-foreground">
              Showing {paginatedMembers.length} of {filteredMembers.length}{" "}
              members
            </div>
            <Pagination
              currentPage={currentPage}
              totalPages={totalPages}
              onPageChange={setCurrentPage}
            />
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
