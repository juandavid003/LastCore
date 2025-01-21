using DB;
using System;
using System.Collections.Generic;
using System.Linq;

public class PatientRepository
{
    private readonly DB_CrudLogInEntities _dbContext;

    public PatientRepository(string connectionString)
    {
        _dbContext = new DB_CrudLogInEntities(connectionString);
    }

    public IEnumerable<Patient> GetAllPatients()
    {
        return _dbContext.patients
            .Select(patient => new Patient
            {
                id = patient.id,
                code = patient.code,
                firstName = patient.firstName,
                lastName = patient.lastName,
                birthDate = patient.birthDate,
                CI = patient.CI,
                medicalHistory = patient.medicalHistory
            })
            .ToList();
    }

    public Patient GetPatientById(int id)
    {
        var patient = _dbContext.patients.FirstOrDefault(p => p.id == id);
        if (patient == null)
            return null;

        return new Patient
        {
            id = patient.id,
            code = patient.code,
            firstName = patient.firstName,
            lastName = patient.lastName,
            birthDate = patient.birthDate,
            CI = patient.CI,
            medicalHistory = patient.medicalHistory
        };
    }

    public void AddPatient(Patient newPatient)
    {
        var patient = new patient
        {
            code = newPatient.code,
            firstName = newPatient.firstName,
            lastName = newPatient.lastName,
            birthDate = newPatient.birthDate,
            CI = newPatient.CI,
            medicalHistory = newPatient.medicalHistory
        };

        _dbContext.patients.Add(patient);
        _dbContext.SaveChanges();
    }

    public bool UpdatePatient(int id, Patient updatedPatient)
    {
        var existingPatient = _dbContext.patients.FirstOrDefault(p => p.id == id);
        if (existingPatient == null)
            return false;

        existingPatient.code = updatedPatient.code;
        existingPatient.firstName = updatedPatient.firstName;
        existingPatient.lastName = updatedPatient.lastName;
        existingPatient.birthDate = updatedPatient.birthDate;
        existingPatient.CI = updatedPatient.CI;
        existingPatient.medicalHistory = updatedPatient.medicalHistory;

        _dbContext.SaveChanges();
        return true;
    }

    public bool DeletePatient(int id)
    {
        var patient = _dbContext.patients.FirstOrDefault(p => p.id == id);
        if (patient == null)
            return false;

        _dbContext.patients.Remove(patient);
        _dbContext.SaveChanges();
        return true;
    }
}
