# Call Center Request Handling Simulation

## 📌 Project Overview
This project is a Call Center Request Handling Application developed for the ONT412 Assignment.

The system simulates how customer requests are processed in a call center environment:
- Requests enter at the **agent level**
- If unresolved, they are escalated to a **supervisor**
- Finally, they may reach a **manager** for resolution

The project demonstrates the implementation of the **Chain of Responsibility design pattern** using C# and .NET Console Application, showcasing decoupled request routing and flexible handler chains.

---

## 👥 Group Members

| Name      | Responsibility |
|-----------|----------------|
| Rohit     | Iterator Design Pattern |
| Marcellos | State Design Pattern |
| Amity     | Composite Design Pattern |
| Bantu     | Proxy Design Pattern |
| Jodi      | Documentation |

---

## 🛠️ Technologies Used

- C#
- .NET 8
- Visual Studio 2022
- GitHub

---

## 🎯 Design Pattern Implemented

| Design Pattern | Purpose |
|----------------|---------|
| **Iterator** | Allows traversal through lists of calls, agents, or requests without exposing internal structure. | Provides clean sequential access to call records and staff lists. |
| **State** | Manages call states such as *On Call*, *On Hold*, *Resolved*, or *Escalated*. | Models real-world call transitions and behavior changes. |
| **Proxy** | Controls access to call logs or restricted actions (e.g., manager-only operations). | Adds a layer of security and controlled access in the system. |
| **Composite** | Represents hierarchical structures (e.g., grouping multiple calls or organizing staff under departments). | Enables treating individual and grouped entities uniformly. |

---

## 📂 Project Structure

```text
CallCenterRequestSystem
│
├── Handlers
│   ├── AgentHandler.cs
│   ├── SupervisorHandler.cs
│   ├── ManagerHandler.cs
│
├── Models
├── Interfaces
└── Program.cs
