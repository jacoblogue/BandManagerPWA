import ExistingEventModel from "./ExistingEventModel";

export default interface SignalRMessage {
  type: string;
  eventId: string;
  event: ExistingEventModel;
}
