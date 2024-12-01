interface UserCardProps {
    user: any;
    onEdit: () => void;
    onDelete: () => void;
  }
  
  const UserCard: React.FC<UserCardProps> = ({ user, onEdit, onDelete }) => (
    <div className="col-md-4 mb-3">
      <div className="card">
        <div className="card-body">
          <h5 className="card-title">{user.username}</h5>
          <p className="card-text">
            <strong>Date Joined:</strong> {user.dateJoined} <br />
            <strong>Email:</strong> {user.email}
          </p>
          <div className="d-flex justify-content-between">
            <button className="btn btn-primary" onClick={onEdit}>
              Edit
            </button>
            <button className="btn btn-danger" onClick={onDelete}>
              Delete
            </button>
          </div>
        </div>
      </div>
    </div>
  );
  
  export default UserCard;
  