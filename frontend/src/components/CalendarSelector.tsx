// import Background from "./components/background"
// import Header from "./components/header"
import { Calendar, Whisper, Popover, Badge } from 'rsuite';
import 'rsuite/dist/rsuite.min.css';

function getTodoList(date:any) {
  const day = date.getDate();

  switch (day) {
    case 10:
      return [
        { time: '10:30 am', title: 'Meeting' },
        { time: '12:00 pm', title: 'Lunch' }
      ];
    case 15:
      return [
        { time: '09:30 pm', title: 'Products Introduction Meeting' },
        { time: '12:30 pm', title: 'Client entertaining' },
        { time: '02:00 pm', title: 'Product design discussion' },
        { time: '05:00 pm', title: 'Product test and acceptance' },
        { time: '06:30 pm', title: 'Reporting' },
        { time: '10:00 pm', title: 'Going home to walk the dog' }
      ];
    default:
      return [];
  }
}

const CalendarSelector = () => {
    const renderCell = (date:any) => {
        const list = getTodoList(date);
        const displayList = list.filter((item, index) => index < 2);
    
        if (list.length) {
          const moreCount = list.length - displayList.length;
          const moreItem = (
            <li>
              <Whisper
                placement="top"
                trigger="click"
                speaker={
                  <Popover>
                    {list.map((item, index) => (
                      <p key={index}>
                        <b>{item.time}</b> - {item.title}
                      </p>
                    ))}
                  </Popover>
                }
              >
                <a>{moreCount} more</a>
              </Whisper>
            </li>
          );
    
          return (
            <div className="overflow-scroll">
                <ul className="calendar-todo-list text-xs">
                {displayList.map((item, index) => (
                    <li key={index}>
                    <Badge /> <b>{item.time}</b> - {item.title}
                    </li>
                ))}
                {moreCount ? moreItem : null}
                </ul>
            </div>
          );
        }
    
        return null;
    }
    
    return <>
        {/* <div style={{background: 'rgba(255,255,255,0.8)'}} className="rounded-xl"/> */}
        <div className="bg-white rounded-xl">
            <Calendar bordered renderCell={renderCell} style={{borderRadius: 30}} />;
        </div>
    </>
}

export default CalendarSelector