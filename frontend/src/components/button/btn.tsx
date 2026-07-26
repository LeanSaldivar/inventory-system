import "./btn.scss"

type LoginButtonProps = {
  text: string;
  icon?: React.ReactNode;
  onClick?: () => void;
};

export default function AuthBtn({
  text,
  onClick,
}: LoginButtonProps) {
  return (
    <button onClick={onClick} className="btn auth">
      <p>
        {text}
      </p>
    </button>
  );
}

export const LogoutBtn = ({
  text,
  icon,
  onClick,
}: LoginButtonProps) => {
  return (
    <button onClick={onClick} className="logoutbtn">
      {icon}
      <span>{text}</span>
    </button>
  );
};