import ExistingEventModel from "@/models/ExistingEventModel";
import NewEventModel from "@/models/NewEventModel";
import axios from "axios";
import { setYear, setMonth, setDate, setHours, setMinutes } from "date-fns";
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

  const customHandleChange = (
    event: React.ChangeEvent<HTMLInputElement>,
    formValues: any,
    setFieldValue: (field: string, value: any, shouldValidate?: boolean) => void
  ) => {
    const { name, value } = event.target;

    if ((name === "date" || name === "time") && formValues.date) {
      let date = new Date(formValues.date);

      if (name === "date") {
        const [year, month, day] = value.split("-").map(Number);
        date = setYear(date, year);
        date = setMonth(date, month - 1);
        date = setDate(date, day);
        setFieldValue("date", date);
      } else if (name === "time") {
        const [hours, minutes] = value.split(":").map(Number);
        date = setHours(date, hours);
        date = setMinutes(date, minutes);

        setFieldValue(
          "time",
          `${String(date.getHours()).padStart(2, "0")}:${String(
            date.getMinutes()
          ).padStart(2, "0")}`
        );
      }
    } else {
      // Update other fields in the form
      setFieldValue(name, value);
    }
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
        console.log("values", values);

        createEvent(values);
        onFormSubmit();
      }}
    >
      {(formik) => (
        <Form
          onSubmit={(e) => {
            e.preventDefault();
            formik.handleSubmit(e);
          }}
        >
          <FormGroup>
            <Label for="title">Title</Label>
            <Field name="title">
              {({ field, form, meta }) => {
                return (
                  <div>
                    <Input
                      {...field}
                      type="text"
                      name="title"
                      id="title"
                      placeholder="Enter the event title"
                      invalid={meta.touched && meta.error ? true : false}
                      valid={meta.touched && !meta.error ? true : false}
                      onChange={(event) => {
                        customHandleChange(
                          event,
                          form.values,
                          formik.setFieldValue
                        );
                      }}
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
              {({ field, form, meta }) => {
                return (
                  <div>
                    <Input
                      {...field}
                      type="textarea"
                      name="description"
                      id="description"
                      placeholder="Enter the event description"
                      invalid={meta.touched && meta.error ? true : false}
                      valid={meta.touched && !meta.error ? true : false}
                      onChange={(event) => {
                        customHandleChange(
                          event,
                          form.values,
                          formik.setFieldValue
                        );
                      }}
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
              {({ field, form, meta }) => {
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
                      placeholder="Enter the event date"
                      value={dateValue}
                      invalid={meta.touched && meta.error ? true : false}
                      valid={meta.touched && !meta.error ? true : false}
                      onChange={(event) => {
                        customHandleChange(
                          event,
                          form.values,
                          formik.setFieldValue
                        );
                      }}
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
              {({ field, form, meta }) => {
                return (
                  <div>
                    <Input
                      {...field}
                      type="time"
                      name="time"
                      id="time"
                      invalid={meta.touched && meta.error ? true : false}
                      value={formik.values.time}
                      placeholder="Enter the event time"
                      valid={meta.touched && !meta.error ? true : false}
                      onChange={(event) => {
                        customHandleChange(
                          event,
                          form.values,
                          formik.setFieldValue
                        );
                      }}
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
              {({ field, form, meta }) => {
                return (
                  <div>
                    <Input
                      {...field}
                      type="text"
                      name="location"
                      id="location"
                      placeholder="Enter the event location"
                      invalid={meta.touched && meta.error ? true : false}
                      valid={meta.touched && !meta.error ? true : false}
                      onChange={(event) => {
                        customHandleChange(
                          event,
                          form.values,
                          formik.setFieldValue
                        );
                      }}
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
