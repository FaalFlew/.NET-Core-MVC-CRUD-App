import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import AddUserForm from "../../components/Dashboard/AddUserForm";
import EditUserForm from "../../components/Dashboard/EditUserForm";
import UserCard from "../../components/Dashboard/UserCard";

const Dashboard: React.FC = () => {
  const navigate = useNavigate();
  const [data, setData] = useState<Array<any>>([]);
  const [newRecord, setNewRecord] = useState({
    username: "",
    dateJoined: "",
    email: "",
    password: "",
  });
  const [isLoading, setIsLoading] = useState(true);
  const [editRecord, setEditRecord] = useState<any | null>(null);

  useEffect(() => {
    const validateSession = async () => {
      const isAuthenticated = localStorage.getItem("isAuthenticated");

      if (!isAuthenticated) {
        navigate("/");
        return;
      }

      try {
        const response = await fetch(
          "http://localhost:5004/api/account/validate-session",
          {
            credentials: "include",
          }
        );

        if (!response.ok) {
          localStorage.removeItem("isAuthenticated");
          navigate("/");
        } else {
          const userResponse = await fetch(
            "http://localhost:5004/api/account/users",
            {
              method: "GET",
              credentials: "include",
            }
          );

          if (userResponse.ok) {
            const users = await userResponse.json();
            setData(users);
          } else {
            console.error("Failed to fetch users");
          }
        }
      } catch (error) {
        console.error("Session validation error:", error);
        localStorage.removeItem("isAuthenticated");
        navigate("/");
      } finally {
        setIsLoading(false);
      }
    };

    validateSession();
  }, [navigate]);

  const handleLogout = async () => {
    await fetch("http://localhost:5004/api/account/logout", {
      method: "POST",
      credentials: "include",
    });
    localStorage.removeItem("isAuthenticated");
    navigate("/");
  };

  const validateEmail = (email: string) => {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailRegex.test(email);
  };

  const handleAddRecord = async (record: typeof newRecord) => {
    if (!validateEmail(record.email)) {
      alert("Enter a valid email address.");
      return;
    }

    const response = await fetch("http://localhost:5004/api/account/users", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(record),
      credentials: "include",
    });

    if (response.ok) {
      const addedUser = await response.json();
      setData((prevData) => [...prevData, addedUser]);
    } else {
      alert("Error adding user");
    }
  };

  const handleSaveEdit = async (updatedRecord: any) => {
    if (!validateEmail(updatedRecord.email)) {
      alert("Enter a valid email address.");
      return;
    }

    const response = await fetch(
      `http://localhost:5004/api/account/users/${updatedRecord.id}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(updatedRecord),
        credentials: "include",
      }
    );

    if (response.ok) {
      setData((prevData) =>
        prevData.map((item) =>
          item.id === updatedRecord.id ? { ...item, ...updatedRecord } : item
        )
      );
      setEditRecord(null);
    } else {
      alert("Error updating user");
    }
  };

  const handleDelete = async (id: number) => {
    const response = await fetch(
      `http://localhost:5004/api/account/users/${id}`,
      {
        method: "DELETE",
        credentials: "include",
      }
    );

    if (response.ok) {
      setData((prevData) => prevData.filter((item) => item.id !== id));
    } else {
      alert("Error deleting user");
    }
  };

  return (
    <div className="container mt-5">
      <h2>User Dashboard</h2>
      <button
        className="btn btn-danger float-right mb-3"
        onClick={handleLogout}
      >
        Log Out
      </button>
      {isLoading ? (
        <p>Loading...</p>
      ) : (
        <>
          <div className="row">
            {data.map((user) => (
              <UserCard
                key={user.id}
                user={user}
                onEdit={() => setEditRecord(user)}
                onDelete={() => handleDelete(user.id)}
              />
            ))}
            {editRecord && (
              <EditUserForm
                record={editRecord}
                onSave={handleSaveEdit}
                onCancel={() => setEditRecord(null)}
                validateEmail={validateEmail}
              />
            )}
          </div>
          <AddUserForm
            record={newRecord}
            onAdd={handleAddRecord}
            validateEmail={validateEmail}
          />
        </>
      )}
    </div>
  );
};

export default Dashboard;
