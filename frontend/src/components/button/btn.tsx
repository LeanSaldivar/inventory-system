import "./btn.scss"

type ButtonProps = {
  text: string;
  icon?: React.ReactNode;
  onClick?: () => void;
};

export default function AuthBtn({
  text,
  onClick,
}: ButtonProps) {
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
}: ButtonProps) => {
  return (
    <button onClick={onClick} className="logoutbtn">
      {icon}
      <span>{text}</span>
    </button>
  );
};

export const FilterBtn = ({
  text,
  onClick,
}: ButtonProps) => {
  return (
    <button onClick={onClick} className="filter">
      <p>
        {text}
      </p>
    </button>
  )
}

export const AiBtn = ({
  text,
  onClick,
}: ButtonProps) => {
  return (
    <button  onClick={onClick} className="aiBtn">
      <p>
        {text}
      </p>
    </button>
  )
}

export const chevronBtn = ({
  icon,
  onClick
}: ButtonProps) => (
  return (
    <button onClick={onClick} className="aiBtn">
      <p>
        {icon}
      </p>
    </button>
  )
)

// export const promptBtn ({
//   icon,
//   onClick
// }: ButtonProps) => {
//   return (
//     <button onClick={onClick} className="prompt">
//       {icon}
//     </button>
//   )
// }