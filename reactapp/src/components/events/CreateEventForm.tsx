import ExistingEventModel from "@/models/ExistingEventModel";
import NewEventModel from "@/models/NewEventModel";
import axios from "axios";
import { Field, Formik, useFormik } from "formik";
import React from "react";
import {
  Button,
  Form,
  FormFeedback,
  FormGroup,
  Input,
  Label,
} from "reactstrap";
import * as Yup from "yup";

interface Props {
  setEvents: React.Dispatch<React.SetStateAction<ExistingEventModel[]>>;
  events: ExistingEventModel[];
  onFormSubmit: () => void;
}
const todayAtMidnight = new Date();
todayAtMidnight.setHours(0, 0, 0, 0);
const validationSchema = Yup.object({
  title: Yup.string()
    .required("Title is required")
    .min(3, "Title should be at least 3 characters")
    .max(100, "Title should not exceed 100 characters"),
  description: Yup.string()
    .optional()
    .max(500, "Description should not exceed 500 characters"),
  date: Yup.date()
    .required("Date is required")
    .min(todayAtMidnight, "Event date should not be in the past"),
  time: Yup.string().required("Time is required"),
  location: Yup.string()
    .required("Location is required")
    .min(3, "Location should be at least 3 characters")
    .max(100, "Location should not exceed 100 characters"),
});

export default function CreateEventForm({
  setEvents,
  events,
  onFormSubmit,
}: Props) {
  const [newEvent, setNewEvent] = React.useState<NewEventModel>({
    title: "",
    description: "",
    date: new Date(),
    location: "",
  });

  const createEvent = (newEvent: NewEventModel) => {
    axios.post("/api/event", newEvent).then((response) => {
      setEvents([...events, response.data]);
    });
  };

  const customHandleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setNewEvent((prevState) => {
      // Clone the existing state
      const updatedState: NewEventModel = { ...prevState };

      if ((name === "date" || name === "time") && prevState.date) {
        // Clone the existing Date object
        const date = new Date(prevState.date.getTime());

        if (name === "date") {
          console.log("date", value);
          const [year, month, day] = value.split("-");
          date.setFullYear(+year, +month - 1, +day);
        } else if (name === "time") {
          console.log("time", value);
          const [hours, minutes] = value.split(":");
          date.setHours(+hours, +minutes);
        }
        // Update the date field in the state
        updatedState.date = date;
      } else {
        // Update other fields in the state
        (updatedState as any)[name] = value;
      }

      return updatedState;
    });
  };

  return (
    <Formik
      initialValues={{
        title: "",
        description: "",
        date: new Date(),
        time: new Date().toLocaleTimeString([], {
          hour: "2-digit",
          minute: "2-digit",
          hour12: false,
        }),
        location: "",
      }}
      validationSchema={validationSchema}
      validateOnBlur={true}
      onSubmit={(values: NewEventModel) => {
        createEvent(values);
        onFormSubmit();
      }}
    >
      {(formik) => (
        <Form>
          <FormGroup>
            <Label for="title">Title</Label>
            <Field name="title">
              {({ field, meta }) => {
                return (
                  <div>
                    <Input
                      {...field}
                      type="text"
                      name="title"
                      id="title"
                      placeholder="Enter the event title"
                      invalid={meta.touched && meta.error}
                      valid={meta.touched && !meta.error}
                    />
                    <FormFeedback>{meta.error}</FormFeedback>
                  </div>
                );
              }}
            </Field>
          </FormGroup>
          <FormGroup>
            <Label for="description">Description</Label>
            <Field name="description">
              {({ field, meta }) => {
                return (
                  <div>
                    <Input
                      {...field}
                      type="textarea"
                      name="description"
                      id="description"
                      placeholder="Enter the event description"
                      invalid={meta.touched && meta.error}
                      valid={meta.touched && !meta.error}
                    />
                    <FormFeedback>{meta.error}</FormFeedback>
                  </div>
                );
              }}
            </Field>
            {formik.errors.description && formik.touched.description ? (
              <div>{formik.errors.description}</div>
            ) : null}
          </FormGroup>
          <FormGroup>
            <Label for="date">Date</Label>
            <Field name="date">
              {({ field, meta }) => {
                const dateValue =
                  field.value instanceof Date
                    ? field.value.toISOString().substring(0, 10)
                    : field.value;
                return (
                  <div>
                    <Input
                      {...field}
                      type="date"
                      name="date"
                      id="date"
                      invalid={meta.touched && meta.error}
                      placeholder="Enter the event date"
                      value={dateValue}
                      valid={meta.touched && !meta.error}
                    />
                    <FormFeedback>{meta.error}</FormFeedback>
                  </div>
                );
              }}
            </Field>
          </FormGroup>
          <FormGroup>
            <Label for="time">Time</Label>
            <Field name="time">
              {({ field, meta }) => {
                return (
                  <div>
                    <Input
                      {...field}
                      type="time"
                      name="time"
                      id="time"
                      invalid={meta.touched && meta.error}
                      placeholder="Enter the event time"
                      valid={meta.touched && !meta.error}
                    />
                    <FormFeedback>{meta.error}</FormFeedback>
                  </div>
                );
              }}
            </Field>
          </FormGroup>
          <FormGroup>
            <Label for="location">Location</Label>
            <Field name="location">
              {({ field, meta }) => {
                return (
                  <div>
                    <Input
                      {...field}
                      type="text"
                      name="location"
                      id="location"
                      placeholder="Enter the event location"
                      invalid={meta.touched && meta.error}
                      valid={meta.touched && !meta.error}
                    />
                    <FormFeedback>{meta.error}</FormFeedback>
                  </div>
                );
              }}
            </Field>
          </FormGroup>
          <Button type="submit">Create Event</Button>
        </Form>
      )}
    </Formik>
  );
}
