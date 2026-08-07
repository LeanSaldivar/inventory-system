import { ChevronRight } from "../../assets/svg/chevronRight";
import { SparklesIcon } from "../../assets/svg/sparklesIcon";
import { AiBtn } from "../button/btn";
import "./aiHeader.scss"

export const AiHeader = () => {
    return (
        <>
            <div className="ai-header">
                <div className="text-wrapper">
                    <div className="ai-logo">
                        <SparklesIcon width={20} height={20} className="ai-icon" />
                    </div>

                    <div className="text">
                        <p>
                            AI Insights
                        </p>

                        <span>
                            Powered by Reisa AI
                        </span>
                    </div>
                </div>

                <div className="dropdown">
                    <div className="online-wrapper">
                        <div className="circle"> </div>

                        <span>
                            AI Online
                        </span>
                    </div>

                    <ChevronRight width={20} height={20} className="down-icon" />
                </div>
            </div>

            <div className="ai-subheader">
                <div className="button-wrapper">
                    <AiBtn path="/reisa/dashboard/reccomend" text={"Reccomendations"} />

                    <AiBtn path="/reisa/dashboard/prompt" text={"Ask AI"} />

                </div>
            </div>

            <div className="ai-content">
                <div className="card">

                </div>
            </div>
        </>
    )
}